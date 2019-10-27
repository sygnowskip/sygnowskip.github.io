using System;
using Common.Docker;
using Common.Tests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ConfigurationProvider = Common.Configuration.ConfigurationProvider;

namespace NaturalIdentifiers.Tests
{
    public class ServiceProviderBuilder
    {
        public static IServiceProvider Build(Action<IServiceCollection> configureServicesAction = null)
        {
            var serviceCollection = new ServiceCollection();
            Configure(serviceCollection);
            configureServicesAction?.Invoke(serviceCollection);
            return serviceCollection.BuildServiceProvider();
        }

        private static void Configure(ServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IConfiguration>(provider => ConfigurationProvider.Get());
            serviceCollection.AddLogging(builder => builder.AddNUnitConsoleOutput());
            serviceCollection.AddDockerSetup();
        }
    }
}