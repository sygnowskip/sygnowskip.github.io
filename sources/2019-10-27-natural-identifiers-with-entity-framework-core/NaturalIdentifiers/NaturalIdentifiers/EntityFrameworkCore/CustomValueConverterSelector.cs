using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace NaturalIdentifiers.EntityFrameworkCore
{
    public class CustomValueConverterSelector : ValueConverterSelector
    {
        private readonly ConcurrentDictionary<(Type ModelClrType, Type ProviderClrType), ValueConverterInfo> _converters
            = new ConcurrentDictionary<(Type ModelClrType, Type ProviderClrType), ValueConverterInfo>();

        public CustomValueConverterSelector(ValueConverterSelectorDependencies dependencies)
            : base(dependencies)
        {
        }

        public override IEnumerable<ValueConverterInfo> Select(Type modelClrType, Type providerClrType = null)
        {
            var baseConverters = base.Select(modelClrType, providerClrType);
            foreach (var converter in baseConverters)
            {
                yield return converter;
            }

            var underlyingModelType = UnwrapNullableType(modelClrType);
            var underlyingProviderType = UnwrapNullableType(providerClrType);

            if (underlyingProviderType == null || underlyingProviderType == typeof(Guid))
            {
                var isIdentifier = typeof(Identifier).IsAssignableFrom(underlyingModelType);
                if (isIdentifier)
                {
                    var converterType = typeof(IdentifierValueConverter<>).MakeGenericType(underlyingModelType);

                    yield return _converters.GetOrAdd((underlyingModelType, typeof(Guid)), _ =>
                    {
                        return new ValueConverterInfo(
                            modelClrType: modelClrType,
                            providerClrType: typeof(Guid),
                            factory: valueConverterInfo => (ValueConverter)Activator.CreateInstance(converterType, valueConverterInfo.MappingHints));
                    });
                }
            }
        }

        private static Type UnwrapNullableType(Type type) => type == null ? null : Nullable.GetUnderlyingType(type) ?? type;
    }
}