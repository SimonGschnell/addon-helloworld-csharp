namespace ResourceFetcher.Services;

public interface IFetchServiceCollection
{
    public List<IFetchService> services { get; set; }
}