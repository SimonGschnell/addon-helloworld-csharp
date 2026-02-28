using dotenv.net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace StremioAddonExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotEnv.Load();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
