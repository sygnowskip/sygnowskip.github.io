using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace NaturalIdentifiers.EntityFrameworkCore
{
    public class IdentifierValueConverter<TIdentifier> : ValueConverter<TIdentifier, Guid>
        where TIdentifier : Identifier
    {
        public IdentifierValueConverter(ConverterMappingHints mappingHints = null)
            : base(id => id.Value, value => Create(value), mappingHints)
        {
        }

        private static TIdentifier Create(Guid id) => Activator.CreateInstance(typeof(TIdentifier), id) as TIdentifier;
    }
}