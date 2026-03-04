using ResourceFetcher.CronJobs;
using StremioAddon.Models;

namespace ResourceFetcher.Services;

public interface IFetchService
{
    public string Name { get; }
    CatalogId CatalogId { get; }
    public HttpRequestMessage GetRequest(CatalogType type);
    public List<IMeta> ConvertToMetaData(string movieResponse);
}