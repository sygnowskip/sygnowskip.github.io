using System;

namespace NaturalIdentifiers.EntityFrameworkCore
{
    public class Identifier
    {
        protected Identifier(Guid value)
        {
            Value = value;
        }

        public Guid Value { get; private set; }
    }
}