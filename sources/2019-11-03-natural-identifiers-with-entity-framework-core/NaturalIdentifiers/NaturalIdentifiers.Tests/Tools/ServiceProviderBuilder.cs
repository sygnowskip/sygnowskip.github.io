using System;
using Common.Docker;
using Common.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NaturalIdentifiers.EntityFrameworkCore.Annotations;
using NaturalIdentifiers.EntityFrameworkCore.Guid;
using NaturalIdentifiers.Persistence;
using ConfigurationProvider = Common.Configuration.ConfigurationProvider;

namespace NaturalIdentifiers.Tests.Tools
{
    public class ServiceProviderBuilder
    {
        public static IServiceProvider Build(Action<IServiceCollection, IConfiguration> configureServicesAction = null, ILoggerFactory loggerFactory = null)
        {
            var configuration = ConfigurationProvider.Get();
            var serviceCollection = new ServiceCollection();
            Configure(serviceCollection, configuration, loggerFactory);
            configureServicesAction?.Invoke(serviceCollection, configuration);
            return serviceCollection.BuildServiceProvider();
        }

        private static void Configure(ServiceCollection serviceCollection, IConfiguration configuration, ILoggerFactory loggerFactory = null)
        {
            serviceCollection.AddDbContext<ApplicationDbContext>(builder =>
                ConfigureDbContextBuilder(builder, configuration, loggerFactory), ServiceLifetime.Transient);
            serviceCollection.AddSingleton<IConfiguration>(provider => configuration);
            serviceCollection.AddLogging(builder => builder.AddNUnitConsoleOutput());
            serviceCollection.AddDockerSetup();
        }

        public static void ConfigureDbContextBuilder(DbContextOptionsBuilder builder, IConfiguration configuration, ILoggerFactory loggerFactory = null)
        {
            builder
                .UseSqlServer(configuration.GetConnectionString(Consts.ConnectionStringName))
                .ReplaceService<IValueConverterSelector, CustomValueConverterSelector>()
                .ReplaceService<IMigrationsAnnotationProvider, CustomAnnotationProvider>()
                .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));

            if (loggerFactory != null)
            {
                builder
                    .UseLoggerFactory(loggerFactory)
                    .EnableSensitiveDataLogging();
            }
        }
    }
}