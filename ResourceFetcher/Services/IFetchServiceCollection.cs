namespace ResourceFetcher.Services;

public interface IFetchServiceCollection
{
    public List<IFetchService> Services { get; set; }
}