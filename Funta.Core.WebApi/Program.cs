using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Funta.Core.Web.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json", optional: false, reloadOnChange: true)
                .Build();

            return WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(config)
                .UseKestrel(options =>
                {
                    options.Listen(IPAddress.Any, 3030);
                })

                .UseStartup<Startup>()
                .UseDefaultServiceProvider(options =>
                          options.ValidateScopes = false)
                .ConfigureAppConfiguration((hostingContext, cfg) =>
                {
                    var env = hostingContext.HostingEnvironment;

                    cfg.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                       .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                    if (env.IsEnvironment("Localapi"))
                    {

                    }

                    cfg.AddEnvironmentVariables();

                    if (args != null)
                    {
                        cfg.AddCommandLine(args);
                    }
                })
                .ConfigureLogging((hostingContext, logging) =>
                {

                })
                .UseIISIntegration();
        }
    }
}
