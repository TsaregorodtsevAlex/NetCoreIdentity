using System.IO;
using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace NetCoreIdentity
{
    public class Program
    {
        private const string UrlParameter = "AppSettings:Url";
        private const string PortParameter = "AppSettings:Port";
        private const string CertUrlParameter = "AppSettings:CertUrl";
        private const string CertPassParameter = "AppSettings:CertPass";

        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var configuration = new ConfigurationBuilder().AddCommandLine(args).Build();

            var host = string.IsNullOrEmpty(configuration[UrlParameter])
                ? "https://localhost"
                : configuration[UrlParameter];

            var port = string.IsNullOrEmpty(configuration[PortParameter])
                ? 44315
                : int.Parse(configuration[PortParameter]);

            var certUrl = string.IsNullOrEmpty(configuration[CertUrlParameter])
                ? "wwwroot/Certs/localhost.pfx"
                : configuration[CertUrlParameter];

            var certPass = string.IsNullOrEmpty(configuration[CertPassParameter])
                ? "123"
                : configuration[CertPassParameter];

            return WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options =>
                {
                    options.Listen(IPAddress.Any, port, listenOptions =>
                    {
                        listenOptions.UseHttps(Path.GetFullPath(certUrl), certPass);
                    });
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseUrls($"{host}:{port}")
                .Build();
        }
    }
}
