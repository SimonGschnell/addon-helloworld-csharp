using Newtonsoft.Json;
using Quartz;
using ResourceFetcher.Models;
using StremioAddonExample.Models;

namespace ResourceFetcher.CronJobs;

public class NetflixFetch: IJob
{
    private readonly ILogger<NetflixFetch> _logger;
    private readonly ResourceFetcherHttpClient _client;

    public NetflixFetch(ILogger<NetflixFetch> logger, ResourceFetcherHttpClient client)
    {
        _logger = logger;
        _client = client;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            _logger.LogInformation("NetflixFetch job executed at: {time}", DateTime.Now);

            await Process(CatalogType.movie, CatalogId.netflixTop10);
            await Process(CatalogType.series, CatalogId.netflixTop10);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private async Task Process(CatalogType catalogType, CatalogId catalogId)
    {
        var movieResponse = await GetListFor(catalogType);

        NewMethod(movieResponse, catalogType, catalogId);
    }

    private static void NewMethod(string movieResponse, CatalogType catalogType, CatalogId catalogId)
    {
        var showObjects = JsonConvert.DeserializeObject<ShowObject[]>(movieResponse) ?? [];
        var metasList = new List<Meta>();
        foreach (var showObject in showObjects)
        {
            var meta = new Meta()
            {
                Id = showObject.imdbId, 
                Name = showObject.originalTitle,
                Genres = showObject.genres.Select(gen => gen.name).ToArray(),
                Type = showObject.showType,
                Poster = showObject.imageSet.verticalPoster.w240,
            };
            metasList.Add(meta);
        }

        var json = new CatalogModel()
        {
            Metas = metasList.ToArray(),
        };

        var data = JsonConvert.SerializeObject(json);
        var outputPath = Environment.GetEnvironmentVariable("RESOURCE_FETCHER_OUTPUT_PATH");
        if (outputPath == null)
        {
            throw new Exception("RESOURCE_FETCHER_OUTPUT_PATH env not set");
        }

        var directoryPath = Path.Combine(outputPath, catalogType.ToString(), catalogId.ToString());

        var filePath = Path.Combine(directoryPath, "testrun.json");
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        File.WriteAllText(filePath,data);
    }

    private async Task<string> GetListFor( CatalogType type)
    {
        var queryParameters = new Dictionary<string, string>()
        {
            { "country", "AT" },
            { "service", "netflix" },
            { "show_type", type.ToString() }
        };
        var movieResponse = await _client.FetchAsync("https://streaming-availability.p.rapidapi.com/shows/top",queryParameters);
        return movieResponse;
    }
}

internal enum CatalogType
{
    movie,
    series
}