namespace ResourceFetcher;

public interface IResourceFetcherHttpClient
{
    public Task<string> FetchAsync(HttpRequestMessage request);
}

public class ResourceFetcherHttpClient : IResourceFetcherHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ResourceFetcherHttpClient> _logger;

    public ResourceFetcherHttpClient(HttpClient client, ILogger<ResourceFetcherHttpClient> logger)
    {
        _httpClient = client;
        _logger = logger;
    }

    public async Task<string> FetchAsync(HttpRequestMessage request)
    {
        
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("{date} - Fetching {url}", DateTime.Now, request.RequestUri);
        }

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}