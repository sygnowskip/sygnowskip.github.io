using System;
using System.Collections.Generic;
using MassTransit;

namespace NaturalIdentifiers.EntityFrameworkCore.Guid
{
    public class Identifier : ValueObject, IComparable
    {
        protected Identifier(System.Guid value)
        {
            Value = value;
        }

        public System.Guid Value { get; private set; }

        public static TIdentifier New<TIdentifier>() where TIdentifier : Identifier
            => IdentifierActivator.Create(typeof(TIdentifier), NewId.NextGuid()) as TIdentifier;

        public static implicit operator System.Guid(Identifier id) => id.Value;

        public override string ToString() => Value.ToString();

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public int CompareTo(object obj)
        {
            if (!(obj is Identifier identifier))
            {
                throw new ArgumentException($"{obj.GetType()} is not an {nameof(Identifier)}");
            }

            return Value.CompareTo(identifier.Value);
        }
    }
}