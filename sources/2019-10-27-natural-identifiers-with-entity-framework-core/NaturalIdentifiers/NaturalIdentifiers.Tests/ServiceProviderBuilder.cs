using System;
using Common.Docker;
using Common.Tests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NaturalIdentifiers.EntityFrameworkCore;
using NaturalIdentifiers.Persistence;
using ConfigurationProvider = Common.Configuration.ConfigurationProvider;

namespace NaturalIdentifiers.Tests
{
    public class ServiceProviderBuilder
    {
        public static IServiceProvider Build(Action<IServiceCollection, IConfiguration> configureServicesAction = null)
        {
            var configuration = ConfigurationProvider.Get();
            var serviceCollection = new ServiceCollection();
            Configure(serviceCollection, configuration);
            configureServicesAction?.Invoke(serviceCollection, configuration);
            return serviceCollection.BuildServiceProvider();
        }

        private static void Configure(ServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDbContext<ApplicationDbContext>(builder => 
                builder
                    .UseSqlServer(configuration.GetConnectionString(Consts.ConnectionStringName))
                    .ReplaceService<IValueConverterSelector, CustomValueConverterSelector>(), ServiceLifetime.Transient);
            serviceCollection.AddSingleton<IConfiguration>(provider => configuration);
            serviceCollection.AddLogging(builder => builder.AddNUnitConsoleOutput());
            serviceCollection.AddDockerSetup();
        }
    }
}