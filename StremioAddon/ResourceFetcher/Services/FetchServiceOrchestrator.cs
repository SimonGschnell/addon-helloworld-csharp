using Newtonsoft.Json;
using StremioAddon.Models;

namespace ResourceFetcher.Services;

public class FetchServiceOrchestrator
{
    private readonly ILogger<FetchServiceOrchestrator> _logger;
    private readonly IResourceFetcherHttpClient _client;
    private readonly IFetchServiceCollection _fetchServiceCollection;
    private readonly IPersistenceService _persistenceService;

    public FetchServiceOrchestrator(ILogger<FetchServiceOrchestrator> logger, IResourceFetcherHttpClient client, IFetchServiceCollection fetchServiceCollection, IPersistenceService persistenceService )
    {
        _logger = logger;
        _client = client;
        _fetchServiceCollection = fetchServiceCollection;
        _persistenceService = persistenceService;
    }
    
    public async Task FetchServicesAsync()
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
        await _persistenceService.PersistCatalogMetaData(catalogType, fetchService.CatalogId, metadata);
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

    
}