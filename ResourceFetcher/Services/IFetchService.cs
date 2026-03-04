using StremioAddon.Models;

namespace ResourceFetcher.Services;

public interface IFetchService
{
    public string Name { get; }
    CatalogId CatalogId { get; }
    public HttpRequestMessage GetRequest(CatalogType type);
    public List<Meta> ConvertToMetaData(string movieResponse);
}