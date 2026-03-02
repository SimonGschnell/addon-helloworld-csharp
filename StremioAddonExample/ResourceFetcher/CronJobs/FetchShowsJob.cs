using Newtonsoft.Json;
using Quartz;
using ResourceFetcher.Models;
using ResourceFetcher.Models.Adapters;
using StremioAddonExample.Models;

namespace ResourceFetcher.CronJobs;

public class FetchShowsJob: IJob
{
    private readonly ILogger<FetchShowsJob> _logger;
    private readonly ResourceFetcherHttpClient _client;

    public FetchShowsJob(ILogger<FetchShowsJob> logger, ResourceFetcherHttpClient client)
    {
        _logger = logger;
        _client = client;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            _logger.LogInformation("Fetching shows: {time}", DateTime.Now);

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
        var outputPath = Environment.GetEnvironmentVariable("RESOURCE_FETCHER_OUTPUT_PATH");
        if (string.IsNullOrEmpty(outputPath))
        {
            throw new Exception("RESOURCE_FETCHER_OUTPUT_PATH env not set");
        }

        var directoryPath = Path.Combine(outputPath, catalogType.ToString());

        var filePath = Path.Combine(directoryPath, $"{catalogId.ToString()}.json");
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        File.WriteAllText(filePath,data);
    }

    private static string ConvertApiResultToMetaDa(string movieResponse)
    {
        var showObjects = JsonConvert.DeserializeObject<ShowObject[]>(movieResponse) ?? [];
        var metasList = new List<IMeta>();
        
        foreach (var showObject in showObjects)
        {
            var meta = new AdapterForMovieOfTheNight(showObject);
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
