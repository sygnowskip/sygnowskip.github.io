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
    public class OrdersDatabaseOperations
    {

        private readonly IServiceProvider _serviceProvider = ServiceProviderBuilder.Build();
        private Order CreateExampleOrder() => Order.Create("Example order", OrderCustomer.Create("John", "Smith"));

        [Test]
        public void ShouldCreateOrderInDatabase()
        {
            int? result = null;
            var order = CreateExampleOrder();

            using (var dbContext = _serviceProvider.GetService<ApplicationDbContext>())
            {
                dbContext.Add(order);
                result = dbContext.SaveChanges();
            }

            result.Should().NotBeNull();
            result.Should().Be(1, "There should be one entry written to database (INSERT)");
        }


        [Test]
        public void ShouldReadOrderFromDatabase()
        {
            var order = CreateExampleOrder();
            Order readOrder = null;

            using (var dbContext = _serviceProvider.GetService<ApplicationDbContext>())
            {
                dbContext.Add(order);
                dbContext.SaveChanges();
            }

            using (var dbContext = _serviceProvider.GetService<ApplicationDbContext>())
            {
                readOrder = dbContext.Orders.SingleOrDefault(c => c.Id == order.Id);
            }

            readOrder.Should().NotBeNull();
            readOrder.Should().BeEquivalentTo(order, "Object saved in database should be equal to object created in runtime");
        }

        [Test]
        public void ShouldUpdateOrderInDatabase()
        {
            var order = CreateExampleOrder();
            int? result = null;

            using (var dbContext = _serviceProvider.GetService<ApplicationDbContext>())
            {
                dbContext.Add(order);
                dbContext.SaveChanges();
            }

            using (var dbContext = _serviceProvider.GetService<ApplicationDbContext>())
            {
                var orderToUpdate = dbContext.Orders.SingleOrDefault(c => c.Id == order.Id);
                orderToUpdate?.ChangeDescription("New description");
                result = dbContext.SaveChanges();
            }

            result.Should().NotBeNull();
            result.Should().Be(1, "There should be one entry written to database (UPDATE)");
        }

        [Test]
        public void ShouldUpdateMultipleOrdersInDatabase()
        {
            var firstOrder = CreateExampleOrder();
            var secondOrder = CreateExampleOrder();
            int? result = null;

            using (var dbContext = _serviceProvider.GetService<ApplicationDbContext>())
            {
                dbContext.Add(firstOrder);
                dbContext.Add(secondOrder);
                dbContext.SaveChanges();
            }

            using (var dbContext = _serviceProvider.GetService<ApplicationDbContext>())
            {
                var firstOrderToUpdate = dbContext.Orders.SingleOrDefault(c => c.Id == firstOrder.Id);
                firstOrderToUpdate?.ChangeDescription("New description");

                var secondOrderToUpdate = dbContext.Orders.SingleOrDefault(c => c.Id == secondOrder.Id);
                secondOrderToUpdate?.ChangeDescription("New description");
                result = dbContext.SaveChanges();
            }

            result.Should().NotBeNull();
            result.Should().Be(2, "There should be two entries written to database (UPDATE)");
        }

        [Test]
        public void ShouldDeleteOrderFromDatabase()
        {
            int? result = null;
            var order = CreateExampleOrder();

            using (var dbContext = _serviceProvider.GetService<ApplicationDbContext>())
            {
                dbContext.Add(order);
                dbContext.SaveChanges();
            }

            using (var dbContext = _serviceProvider.GetService<ApplicationDbContext>())
            {
                var orderToDelete = dbContext.Orders.SingleOrDefault(c => c.Id == order.Id);
                if (orderToDelete != null)
                {
                    dbContext.Orders.Remove(orderToDelete);
                    result = dbContext.SaveChanges();
                }
            }

            result.Should().NotBeNull();
            result.Should().Be(1, "There should be one entry written to database (DELETE)");
        }
    }
}