using ResourceFetcher.CronJobs;

namespace ResourceFetcher.Services;

public class FetchServiceCollection : IFetchServiceCollection
{
    public List<IFetchService> Services { get; set; } = [];

    public FetchServiceCollection WithNetflixService()
    {
        Services.Add(NetflixService.Create());
        return this;
    }
}