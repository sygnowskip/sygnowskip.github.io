using System.IO;
using Microsoft.Extensions.Configuration;

namespace Common.Configuration
{
    public class ConfigurationProvider
    {
        private ConfigurationProvider() { }

        public static IConfigurationRoot Get()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}