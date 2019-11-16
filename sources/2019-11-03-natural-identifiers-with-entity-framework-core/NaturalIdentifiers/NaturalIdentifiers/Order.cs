using NaturalIdentifiers.EntityFrameworkCore.Numeric;

namespace NaturalIdentifiers
{
    public class OrderId : Identifier
    {
        protected OrderId(long value) : base(value)
        {
        }
    }

    public class Order
    {
        private Order(string description, OrderCustomer customer)
        {
            Description = description;
            Customer = customer;
        }

        protected Order()
        {
        }

        public OrderId Id { get; private set; }
        public string Description { get; private set; }
        public OrderCustomer Customer { get; private set; }
        
        public void ChangeDescription(string description)
        {
            Description = description;
        }

        public static Order Create(string description, OrderCustomer customer)
        {
            return new Order(description, customer);
        }
    }

    public class OrderCustomer
    {
        private OrderCustomer(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public static OrderCustomer Create(string firstName, string lastName)
        {
            return new OrderCustomer(lastName, firstName);
        }
    }
}