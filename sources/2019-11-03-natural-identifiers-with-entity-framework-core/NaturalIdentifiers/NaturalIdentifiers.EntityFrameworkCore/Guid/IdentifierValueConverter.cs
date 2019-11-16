using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace NaturalIdentifiers.EntityFrameworkCore.Guid
{
    internal class IdentifierValueConverter<TIdentifier> : ValueConverter<TIdentifier, System.Guid>
        where TIdentifier : Identifier
    {
        public IdentifierValueConverter()
            : base(id => id.Value, value => Create(value))
        {
        }

        private static TIdentifier Create(System.Guid id) => IdentifierActivator.Create(typeof(TIdentifier), id) as TIdentifier;
    }
}