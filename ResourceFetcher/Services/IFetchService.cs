using ResourceFetcher.Models.Adapters;
using StremioAddon.Models;

namespace ResourceFetcher.Services;

public class PrimeService: MovieOfTheNightServiceBase, IFetchService
{
    private string country { get; }
    public string Name => "PrimeService";
    public CatalogId CatalogId => CatalogId.primeTop10;

    public PrimeService(string country)
    {
        this.country = country;
    }
    
    public HttpRequestMessage GetRequest(CatalogType type)
    {
        return GenerateHttpRequestMessage(type, country, "prime");
    }
}

public class MovieOfTheNightServiceBase
{
    private IMetaAdapter metadataAdapter { get; } = new AdapterForMovieOfTheNight();

    public List<Meta> ConvertToMetaData(string movieResponse) =>
        metadataAdapter.ConvertToStandardizedMetaData(movieResponse);

    protected static HttpRequestMessage GenerateHttpRequestMessage(CatalogType type, string country, string service)
    {
        var queryParameters = new Dictionary<string, string>()
        {
            { "country", country },
            { "service", service },
            { "show_type", type.ToString() }
        };
        const string url = "https://streaming-availability.p.rapidapi.com/shows/top";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var uriBuilder = new UriBuilder(url);
        var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
        foreach (var kv in queryParameters)
        {
            query[kv.Key] = kv.Value;
        }

        uriBuilder.Query = query.ToString();
        request.RequestUri = uriBuilder.Uri;
        return request;
    }
}

public class HboService : MovieOfTheNightServiceBase, IFetchService
{
    private string country { get; }
    public string Name => "HboService";
    public CatalogId CatalogId => CatalogId.hboTop10;

    public HboService(string country)
    {
        this.country = country;
    }

    public HttpRequestMessage GetRequest(CatalogType type)
    {
        return GenerateHttpRequestMessage(type, country, "hbo");
    }
}

public class AppleService : MovieOfTheNightServiceBase, IFetchService
{
    private string country { get; }
    public string Name => "AppleService";
    public CatalogId CatalogId => CatalogId.appleTop10;

    public AppleService(string country)
    {
        this.country = country;
    }

    public HttpRequestMessage GetRequest(CatalogType type)
    {
        return GenerateHttpRequestMessage(type, country, "apple");
    }
}

public class DisneyService : MovieOfTheNightServiceBase, IFetchService
{
    private string country { get; }
    public string Name => "DisneyService";
    public CatalogId CatalogId => CatalogId.disneyTop10;

    public DisneyService(string country)
    {
        this.country = country;
    }

    public HttpRequestMessage GetRequest(CatalogType type)
    {
        return GenerateHttpRequestMessage(type, country, "disney");
    }
}

public class NetflixService : MovieOfTheNightServiceBase, IFetchService
{
    private string country { get; }
    public string Name => "NetflixService";
    public CatalogId CatalogId => CatalogId.netflixTop10;

    public NetflixService(string country)
    {
        this.country = country;
    }

    public HttpRequestMessage GetRequest(CatalogType type)
    {
        return GenerateHttpRequestMessage(type, country, "netflix");
    }
}

public interface IFetchService
{
    public string Name { get; }
    CatalogId CatalogId { get; }
    public HttpRequestMessage GetRequest(CatalogType type);
    public List<Meta> ConvertToMetaData(string movieResponse);
}