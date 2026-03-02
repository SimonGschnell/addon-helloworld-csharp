using ResourceFetcher.CronJobs;
using ResourceFetcher.Models.Adapters;
using StremioAddonExample.Models;

namespace ResourceFetcher.Services;

public class NetflixService: IFetchService
{
    public static NetflixService Create()
    {
        return new NetflixService();
    }

    public string name => "NetflixService";
    public CatalogId catalogId => CatalogId.netflixTop10;
    public IMetaAdapter metadataAdapter { get; set; } = new AdapterForMovieOfTheNight();

    public HttpRequestMessage GetRequest(CatalogType type)
    {
        var queryParameters = new Dictionary<string, string>()
        {
            { "country", "AT" },
            { "service", "netflix" },
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

    public List<IMeta> ConvertToMetaData(string movieResponse) => metadataAdapter.ConvertToStandardizedMetaData(movieResponse);
}