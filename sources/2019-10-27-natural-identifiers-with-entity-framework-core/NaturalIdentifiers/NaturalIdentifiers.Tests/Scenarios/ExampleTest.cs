using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using NaturalIdentifiers.Persistence;
using NUnit.Framework;

namespace NaturalIdentifiers.Tests.Scenarios
{
    [TestFixture]
    public class ExampleTest
    {
        private readonly IServiceProvider _serviceProvider = ServiceProviderBuilder.Build();

        [Test]
        public void ShouldPass()
        {
            using (var dbContext = _serviceProvider.GetService<ApplicationDbContext>())
            {
                var customer = Customer.Create("Jon", "Smith");
                dbContext.Add(customer);
                dbContext.SaveChanges();
            }

            using (var dbContext = _serviceProvider.GetService<ApplicationDbContext>())
            {
                var results = dbContext.Customers.ToList();
            }

            Assert.Pass();
        }
    }
}