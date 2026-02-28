namespace ResourceFetcher;

public class ResourceFetcherHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ResourceFetcherHttpClient> _logger;

    public ResourceFetcherHttpClient(HttpClient client, ILogger<ResourceFetcherHttpClient> logger)
    {
        _httpClient = client;
        _logger = logger;
    }

    public async Task<string> FetchAsync(string url, Dictionary<string, string>? queryParameters = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        if (queryParameters != null)
        {
            var uriBuilder = new UriBuilder(url);
            var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
            foreach (var kv in queryParameters)
            {
                query[kv.Key] = kv.Value;
            }

            uriBuilder.Query = query.ToString();
            request.RequestUri = uriBuilder.Uri;
        }
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("{date} - Fetching {url}", DateTime.Now, request.RequestUri);
        }

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}