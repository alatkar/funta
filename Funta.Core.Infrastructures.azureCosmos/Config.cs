using System.IO;
using Microsoft.Extensions.Configuration;

namespace Funta.Core.Infrastructures.azureCosmos
{
    public class Config
    {
        private static IConfigurationRoot Configuration { get; set; }
        public static string DatabaseName => Configuration[nameof(DatabaseName)];
        public static string DocumentDbPrimaryKey => Configuration[nameof(DocumentDbPrimaryKey)];
        public static string DocumentDbEndpointUrl => Configuration[nameof(DocumentDbEndpointUrl)];

        static Config()
        {
            var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("config.json", optional: true, reloadOnChange: true);
            Configuration = config.Build();
        }
    }
}
