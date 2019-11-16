using Common.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using NaturalIdentifiers.Persistence;
using NaturalIdentifiers.Tests.Tools;

namespace NaturalIdentifiers.Tests
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            ServiceProviderBuilder.ConfigureDbContextBuilder(optionsBuilder,
                ConfigurationProvider.Get());

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}