using System;
using NaturalIdentifiers.EntityFrameworkCore.Guid;

namespace NaturalIdentifiers
{
    public class CustomerId : Identifier
    {
        public CustomerId(Guid value) : base(value)
        {
        }
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

        public void ChangeFirstName(string firstName)
        {
            FirstName = firstName;
        }

        public static Customer Create(string firstName, string lastName, Address address)
        {
            return new Customer(Identifier.New<CustomerId>(), firstName, lastName, address);
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