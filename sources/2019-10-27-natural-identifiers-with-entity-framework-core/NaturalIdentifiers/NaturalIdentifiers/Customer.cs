using System;
using NaturalIdentifiers.EntityFrameworkCore;

namespace NaturalIdentifiers
{
    public class CustomerId : Identifier
    {
        public CustomerId(Guid value) : base(value)
        {
        }

        public static CustomerId New() => new CustomerId(Guid.NewGuid());
    }

    public class Customer
    {
        private Customer(CustomerId id, string firstName, string lastName, Address address)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Address = address;
        }

        protected Customer()
        {
        }

        public CustomerId Id { get; protected set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Address Address { get; private set; }

        public static Customer Create(string firstName, string lastName)
        {
            return new Customer(CustomerId.New(), firstName, lastName, null);
        }
    }

    public class Address
    {
        private Address(string street, string postalCode, string city)
        {
            Street = street;
            PostalCode = postalCode;
            City = city;
        }

        public string Street { get; private set; }
        public string PostalCode { get; private set; }
        public string City { get; private set; }

        public static Address Create(string street, string postalCode, string city)
        {
            return new Address(street, postalCode, city);
        }
    }
}