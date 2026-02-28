using dotenv.net;
using dotenv.net.Utilities;
using ResourceFetcher.CronJobs;

namespace ResourceFetcher;
using Quartz;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        DotEnv.Load(options: new DotEnvOptions(trimValues: true));
        
        builder.Services.AddHttpClient<ResourceFetcherHttpClient>(conf =>
        {
            conf.DefaultRequestHeaders.Add("X-RapidAPI-Key",EnvReader.GetStringValue("RAPID_API_KEY"));
            conf.DefaultRequestHeaders.Add("X-RapidAPI-Host", "streaming-availability.p.rapidapi.com");
        });
        
        builder.Services.AddQuartz(q =>
        {
            var netflixJob = new JobKey("NetflixFetch");
            q.AddJob<NetflixFetch>(opts => opts.WithIdentity(netflixJob));

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