using System.Reflection;
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

            await FetchAndPersistFor(CatalogId.netflixTop10, CatalogType.movie);
            await FetchAndPersistFor(CatalogId.netflixTop10, CatalogType.series);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private async Task FetchAndPersistFor(CatalogId catalogId, CatalogType catalogType)
    {
        var movieResponse = await GetApiResultFor(catalogType);

        var data = ConvertApiResultToMetaDa(movieResponse);

        PersistMetaDataToLocalApplicationData(catalogType, catalogId, data);
    }

    private static void PersistMetaDataToLocalApplicationData(CatalogType catalogType, CatalogId catalogId, string data)
    {
        var projectName = Assembly.GetExecutingAssembly().GetName().Name ?? "ResourceFetcher";
        
        var outputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), projectName);

        var directoryPath = Path.Combine(outputPath, catalogType.ToString(), catalogId.ToString());

        var filePath = Path.Combine(directoryPath, "testrun.json");
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        File.WriteAllText(filePath,data);
    }

    private static string ConvertApiResultToMetaDa(string movieResponse)
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
        return data;
    }

    private async Task<string> GetApiResultFor( CatalogType type)
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