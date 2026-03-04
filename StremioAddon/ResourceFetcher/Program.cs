using dotenv.net;
using ResourceFetcher.CronJobs;
using ResourceFetcher.Services;

namespace ResourceFetcher;
using Quartz;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        DotEnv.Load(options: new DotEnvOptions(trimValues: true, probeForEnv:true, probeLevelsToSearch:5));
        var rapidApiKey = Environment.GetEnvironmentVariable("RAPID_API_KEY");
        if (string.IsNullOrEmpty(rapidApiKey))
        {
            throw new Exception("RAPID_API_KEY env not set");
        }

        builder.Services.AddScoped<IFetchServiceCollection>(_ => 
            new FetchServiceCollection()
                .WithNetflixService()
                .WithPrimeService()
                .WithDisneyService()
                .WithAppleService()
                .WithHboService());
        builder.Services.AddScoped<FetchServiceOrchestrator>();
        builder.Services.AddScoped<IPersistenceService, FilePersistence>();
        builder.Services.AddHttpClient<IResourceFetcherHttpClient,ResourceFetcherHttpClient>(conf =>
        {
            conf.DefaultRequestHeaders.Add("X-RapidAPI-Key",rapidApiKey);
            conf.DefaultRequestHeaders.Add("X-RapidAPI-Host", "streaming-availability.p.rapidapi.com");
        });
        
        builder.Services.AddQuartz(q =>
        {
            var netflixJob = new JobKey("NetflixFetch");
            q.AddJob<FetchShowsJob>(opts => opts.WithIdentity(netflixJob));

            q.AddTrigger(opts => opts
                .ForJob(netflixJob)
                .WithIdentity("NetflixFetchTriggerNow")
                .StartNow());
            q.AddTrigger(opts => opts
                .ForJob(netflixJob)
                .WithIdentity("NetflixFetchTrigger")
                .WithCronSchedule("0 0 8 ? * *", x =>
                {
                    x.InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Europe/Vienna"));
                }));
        });

        builder.Services.AddQuartzHostedService(opts => { opts.WaitForJobsToComplete = true; });
        var host = builder.Build();
        host.Run();
    }
}