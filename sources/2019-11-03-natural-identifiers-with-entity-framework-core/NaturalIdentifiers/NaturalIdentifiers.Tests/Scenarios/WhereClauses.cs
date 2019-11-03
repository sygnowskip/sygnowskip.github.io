using System;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NaturalIdentifiers.Persistence;
using NaturalIdentifiers.Tests.Tools;
using NUnit.Framework;

namespace NaturalIdentifiers.Tests.Scenarios
{
    [TestFixture]
    public class WhereClauses
    {
        private IServiceProvider serviceProvider;
        private SqlQueriesLogger queriesLogger;
        private Customer CreateExampleCustomer() => Customer.Create("Luke", "Skywalker", Address.Create("Tatooine", "R2-D2", "Galaxy"));

        [SetUp]
        public void SetUp()
        {
            queriesLogger = new SqlQueriesLogger();
            serviceProvider = ServiceProviderBuilder.Build(loggerFactory: QueriesProvider.CreateFactory(queriesLogger));
        }

        [Test]
        public void ShouldAddWhereClauseToSelectById()
        {
            var customer = CreateExampleCustomer();

            using (var dbContext = serviceProvider.GetService<ApplicationDbContext>())
            {
                dbContext.Add(customer);
                dbContext.SaveChanges();
            }
            
            using (var dbContext = serviceProvider.GetService<ApplicationDbContext>())
            {
                var selectedCustomer = dbContext.Customers.SingleOrDefault(c => c.Id == customer.Id);
            }

            var selectQuery = queriesLogger.Queries.SingleOrDefault(q => q.Contains("SELECT"));

            selectQuery.Should().Contain(customer.Id.ToString(), $"SELECT query should contain WHERE clause on customer ID {customer.Id}");
        }

        [Test]
        public void ShouldAddWhereClauseToUpdateById()
        {
            var customer = CreateExampleCustomer();

            using (var dbContext = serviceProvider.GetService<ApplicationDbContext>())
            {
                dbContext.Add(customer);
                dbContext.SaveChanges();
            }

            using (var dbContext = serviceProvider.GetService<ApplicationDbContext>())
            {
                var customerToUpdate = dbContext.Customers.SingleOrDefault(c => c.Id == customer.Id);
                if (customerToUpdate != null)
                    customerToUpdate.ChangeFirstName("Anakin");

                dbContext.SaveChanges();
            }

            var selectQuery = queriesLogger.Queries.SingleOrDefault(q => q.Contains("UPDATE"));

            selectQuery.Should().Contain(customer.Id.ToString(), $"UPDATE query should contain WHERE clause on customer ID {customer.Id}");
        }

        [Test]
        public void ShouldAddWhereClauseToDeleteById()
        {

            var customer = CreateExampleCustomer();

            using (var dbContext = serviceProvider.GetService<ApplicationDbContext>())
            {
                dbContext.Add(customer);
                dbContext.SaveChanges();
            }

            using (var dbContext = serviceProvider.GetService<ApplicationDbContext>())
            {
                var customerToDelete = dbContext.Customers.SingleOrDefault(c => c.Id == customer.Id);
                if (customerToDelete != null)
                {
                    dbContext.Customers.Remove(customerToDelete);
                    dbContext.SaveChanges();
                }
            }

            var selectQuery = queriesLogger.Queries.SingleOrDefault(q => q.Contains("DELETE"));

            selectQuery.Should().Contain(customer.Id.ToString(), $"DELETE query should contain WHERE clause on customer ID {customer.Id}");
        }
        
    }
}