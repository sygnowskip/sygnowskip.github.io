using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Database;
using Common.Database.Creator;
using Common.Docker;
using Common.Docker.Resources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace NaturalIdentifiers.Tests
{
    [SetUpFixture]
    public class UnitTestsSetup
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DockerSetup _dockerSetup;

        public UnitTestsSetup()
        {
            _serviceProvider = ServiceProviderBuilder.Build();
            _dockerSetup = DockerSetup.Create(SolutionPathProvider.GetSolutionPath("NaturalIdentifiers").DirectoryName,
                new List<string>()
                {
                    "docker-compose.yml"
                });
        }

        [OneTimeSetUp]
        public async Task SetUp()
        {
            var configuration = _serviceProvider.GetService<IConfiguration>();
            var mssqlAwaiter = _serviceProvider.GetService<MsSqlResourceAwaiter>();
            var connectionString = configuration.GetConnectionString(Consts.ConnectionStringName);

#if RELEASE
            _dockerSetup.Up();
#endif
            Task.WaitAll(
                mssqlAwaiter.WaitForConnectionAsync(connectionString.ToMasterDatabase()));
            
            var logger = _serviceProvider.GetService<ILogger<DatabaseCreator>>();
            
            await DatabaseCreator
                .WithConnectionString(connectionString, logger)
                .CreateAsync();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
#if RELEASE
            _dockerSetup.Down();
#endif
        }
    }
}