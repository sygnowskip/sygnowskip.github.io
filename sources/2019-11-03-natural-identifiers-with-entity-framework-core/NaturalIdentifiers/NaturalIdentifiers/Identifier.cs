using System;
using System.Collections.Generic;
using NaturalIdentifiers.Common;
using NaturalIdentifiers.EntityFrameworkCore;

namespace NaturalIdentifiers
{
    public class Identifier : ValueObject, IComparable
    {
        protected Identifier(Guid value)
        {
            Value = value;
        }

        public Guid Value { get; private set; }

        public static TIdentifier New<TIdentifier>() where TIdentifier : Identifier
            => IdentifierActivator.Create(typeof(TIdentifier), Guid.NewGuid()) as TIdentifier;

        public static implicit operator Guid(Identifier id) => id.Value;

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