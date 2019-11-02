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
    public class DatabaseOperations
    {
        private readonly IServiceProvider _serviceProvider = ServiceProviderBuilder.Build();
        private Customer CreateExampleCustomer() => Customer.Create("Luke", "Skywalker", Address.Create("Tatooine", "R2-D2", "Galaxy"));

        [Test]
        public void ShouldCreateCustomerInDatabase()
        {
            int? result = null;
            var customer = CreateExampleCustomer();

            using (var dbContext = _serviceProvider.GetService<ApplicationDbContext>())
            {
                dbContext.Add(customer);
                result = dbContext.SaveChanges();
            }

            result.Should().NotBeNull();
            result.Should().Be(1, "There should be one entry written to database (INSERT)");
        }

        [Test]
        public void ShouldReadCustomerFromDatabase()
        {
            var customer = CreateExampleCustomer();
            Customer readCustomer = null;

            using (var dbContext = _serviceProvider.GetService<ApplicationDbContext>())
            {
                dbContext.Add(customer);
                dbContext.SaveChanges();
            }

            using (var dbContext = _serviceProvider.GetService<ApplicationDbContext>())
            {
                readCustomer = dbContext.Customers.SingleOrDefault(c => c.Id == customer.Id);
            }

            readCustomer.Should().NotBeNull();
            readCustomer.Should().BeEquivalentTo(customer, "Object saved in database should be equal to object created in runtime");
        }

        [Test]
        public void ShouldUpdateCustomerInDatabase()
        {
            var customer = CreateExampleCustomer();
            int? result = null;

            using (var dbContext = _serviceProvider.GetService<ApplicationDbContext>())
            {
                dbContext.Add(customer);
                dbContext.SaveChanges();
            }

            using (var dbContext = _serviceProvider.GetService<ApplicationDbContext>())
            {
                var customerToUpdate = dbContext.Customers.SingleOrDefault(c => c.Id == customer.Id);
                customerToUpdate?.ChangeFirstName("Anakin");
                result = dbContext.SaveChanges();
            }

            result.Should().NotBeNull();
            result.Should().Be(1, "There should be one entry written to database (UPDATE)");
        }

        [Test]
        public void ShouldUpdateMultipleCustomersInDatabase()
        {
            var firstCustomer = CreateExampleCustomer();
            var secondCustomer = CreateExampleCustomer();
            int? result = null;

            using (var dbContext = _serviceProvider.GetService<ApplicationDbContext>())
            {
                dbContext.Add(firstCustomer);
                dbContext.Add(secondCustomer);
                dbContext.SaveChanges();
            }

            using (var dbContext = _serviceProvider.GetService<ApplicationDbContext>())
            {
                var firstCustomerToUpdate = dbContext.Customers.SingleOrDefault(c => c.Id == firstCustomer.Id);
                firstCustomerToUpdate?.ChangeFirstName("Anakin");

                var secondCustomerToUpdate = dbContext.Customers.SingleOrDefault(c => c.Id == secondCustomer.Id);
                secondCustomerToUpdate?.ChangeFirstName("Leia");
                result = dbContext.SaveChanges();
            }

            result.Should().NotBeNull();
            result.Should().Be(2, "There should be two entries written to database (UPDATE)");
        }

        [Test]
        public void ShouldDeleteCustomerFromDatabase()
        {
            int? result = null;
            var customer = CreateExampleCustomer();

            using (var dbContext = _serviceProvider.GetService<ApplicationDbContext>())
            {
                dbContext.Add(customer);
                dbContext.SaveChanges();
            }

            using (var dbContext = _serviceProvider.GetService<ApplicationDbContext>())
            {
                var customerToDelete = dbContext.Customers.SingleOrDefault(c => c.Id == customer.Id);
                if (customerToDelete != null)
                {
                    dbContext.Customers.Remove(customerToDelete);
                    result = dbContext.SaveChanges();
                }
            }
            
            result.Should().NotBeNull();
            result.Should().Be(1, "There should be one entry written to database (DELETE)");
        }
    }
}