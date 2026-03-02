using Newtonsoft.Json;
using Quartz;
using ResourceFetcher.Helpers;
using ResourceFetcher.Models;
using ResourceFetcher.Services;
using StremioAddonExample.Models;

namespace ResourceFetcher.CronJobs;

public class FetchShowsJob: IJob
{
    private readonly IFetchServiceCollection _fetchServiceCollection;
    private readonly ILogger<FetchShowsJob> _logger;
    private readonly ResourceFetcherHttpClient _client;

    public FetchShowsJob(ILogger<FetchShowsJob> logger, ResourceFetcherHttpClient client, IFetchServiceCollection fetchServiceCollection)
    {
        _fetchServiceCollection = fetchServiceCollection;
        _logger = logger;
        _client = client;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            await FetchServicesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private async Task FetchServicesAsync()
    {
        _logger.LogInformation("Fetching shows: {time}", DateTime.UtcNow);
        foreach (var fetchService in _fetchServiceCollection.Services)
        {
            await FetchAndPersistFor(CatalogType.movie, fetchService);
            await FetchAndPersistFor(CatalogType.series, fetchService);
        }
    }

    private async Task FetchAndPersistFor(CatalogType catalogType, IFetchService fetchService)
    {
        var result = await GetApiResultFor(catalogType, fetchService);
        var metadata = ConvertApiResultToMetaData(result, fetchService);
        await PersistCatalogMetaData(catalogType, fetchService.CatalogId, metadata);
    }

    private async Task<string> GetApiResultFor(CatalogType type, IFetchService fetchService)
    {
        _logger.LogInformation("Fetching {catalogType} for {service}: {time}", type.ToString(), fetchService.Name, DateTime.UtcNow);
        var request = fetchService.GetRequest(type);
        return await _client.FetchAsync(request);
    }

    private static string ConvertApiResultToMetaData(string movieResponse, IFetchService fetchService)
    {
        var metasList = fetchService.ConvertToMetaData(movieResponse);

        return JsonConvert.SerializeObject(new CatalogModel
        {
            Metas = metasList.ToArray(),
        });
    }

    private static async Task PersistCatalogMetaData(CatalogType catalogType, CatalogId catalogId, string data)
    {
        var directoryPath = Path.Combine(EnvironmentHelper.GetOutputPath(), catalogType.ToString());

        var filePath = Path.Combine(directoryPath, $"{catalogId.ToString()}.json");
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        await File.WriteAllTextAsync(filePath, data);
    }
}
