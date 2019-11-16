using System;
using System.Collections.Generic;
using MassTransit;

namespace NaturalIdentifiers.EntityFrameworkCore.Numeric
{
    public class Identifier : ValueObject, IComparable
    {
        protected Identifier(long value)
        {
            Value = value;
        }

        public long Value { get; private set; }

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