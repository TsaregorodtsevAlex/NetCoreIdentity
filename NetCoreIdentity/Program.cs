using System.IO;
using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace NetCoreIdentity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options =>
                {
                    options.Listen(IPAddress.Any, 44315, listenOptions =>
                    {
                        listenOptions.UseHttps(Path.GetFullPath("wwwroot/Certs/vss.pfx"), "1234");
                        //listenOptions.UseHttps(Path.GetFullPath("wwwroot/Certs/localhost.pfx"), "123");
                    });
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                //.UseUrls("https://localhost:44315/")
                .UseUrls("https://192.168.45.99:44315/")
                .Build();
    }
}
