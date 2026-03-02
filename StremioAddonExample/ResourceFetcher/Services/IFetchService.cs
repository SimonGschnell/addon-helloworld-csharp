using ResourceFetcher.CronJobs;
using StremioAddonExample.Models;

namespace ResourceFetcher.Services;

public interface IFetchService
{
    public string Name { get; }
    CatalogId CatalogId { get; }
    public HttpRequestMessage GetRequest(CatalogType type);
    public List<IMeta> ConvertToMetaData(string movieResponse);
}