using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace WebBigData
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DataBaseReader.LinksRead();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}