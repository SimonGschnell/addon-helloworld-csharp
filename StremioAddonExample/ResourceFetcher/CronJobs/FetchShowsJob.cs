using Quartz;
using ResourceFetcher.Services;

namespace ResourceFetcher.CronJobs;

public class FetchShowsJob: IJob
{
    private readonly FetchServiceOrchestrator _fetchServiceOrchestrator;
    private readonly IFetchServiceCollection _fetchServiceCollection;
    private readonly ILogger<FetchShowsJob> _logger;
    private readonly ResourceFetcherHttpClient _client;

    public FetchShowsJob(ILogger<FetchShowsJob> logger, FetchServiceOrchestrator fetchServiceOrchestrator)
    {
        _fetchServiceOrchestrator = fetchServiceOrchestrator;
        _logger = logger;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            await _fetchServiceOrchestrator.FetchServicesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}
