using ResourceFetcher.CronJobs;

namespace ResourceFetcher.Services;

public class FetchServiceCollection : IFetchServiceCollection
{
    public List<IFetchService> services { get; set; } = [];

    public FetchServiceCollection WithNetflixService()
    {
        services.Add(NetflixService.Create());
        return this;
    }
}