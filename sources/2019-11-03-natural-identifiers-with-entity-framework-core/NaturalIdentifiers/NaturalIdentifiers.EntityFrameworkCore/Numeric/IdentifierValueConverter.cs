using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace NaturalIdentifiers.EntityFrameworkCore.Numeric
{
    internal class IdentifierValueConverter<TIdentifier> : ValueConverter<TIdentifier, long>
        where TIdentifier : Numeric.Identifier
    {
        public IdentifierValueConverter()
            : base(id => id.Value, value => Create(value), new ConverterMappingHints(valueGeneratorFactory: (p, t) => new TemporaryIdentifierValueGenerator<TIdentifier>()))
        {
        }

        private static TIdentifier Create(long id) => IdentifierActivator.Create(typeof(TIdentifier), id) as TIdentifier;
        
        private class TemporaryIdentifierValueGenerator<T> : ValueGenerator
        {
            private readonly TemporaryLongValueGenerator _innerGenerator = new TemporaryLongValueGenerator();

            protected override object NextValue(EntityEntry entry)
            {
                return IdentifierActivator.Create(typeof(T), _innerGenerator.Next(entry));
            }

            public override bool GeneratesTemporaryValues => _innerGenerator.GeneratesTemporaryValues;
        }
    }
}