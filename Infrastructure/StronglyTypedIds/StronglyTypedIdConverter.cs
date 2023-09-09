using System.Collections.Concurrent;
using System.ComponentModel;
using System.Globalization;

namespace Infrastructure.StronglyTypedIds;

public class StronglyTypedIdConverter : TypeConverter
{
    private static readonly ConcurrentDictionary<Type, TypeConverter> ActualConverters = new();

    private readonly TypeConverter _innerConverter;

    public StronglyTypedIdConverter(Type stronglyTypedIdType)
    {
        _innerConverter = ActualConverters.GetOrAdd(stronglyTypedIdType, CreateActualConverter);
    }

    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return _innerConverter.CanConvertFrom(context, sourceType);
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
    {
        return _innerConverter.CanConvertTo(context, destinationType);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        return _innerConverter.ConvertFrom(context, culture, value);
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value,
        Type destinationType)
    {
        return _innerConverter.ConvertTo(context, culture, value, destinationType);
    }


    private static TypeConverter CreateActualConverter(Type stronglyTypedIdType)
    {
        if (!StronglyTypedIdHelper.IsStronglyTypedId(stronglyTypedIdType))
        {
            throw new InvalidStronglyTypedIdException($"The type '{stronglyTypedIdType}' is not a strongly typed id");
        }

        var actualConverterType = typeof(StronglyTypedIdConverter<>).MakeGenericType(stronglyTypedIdType);
        return (TypeConverter) Activator.CreateInstance(actualConverterType, stronglyTypedIdType)!;
    }
}

public class StronglyTypedIdConverter<TValue> : TypeConverter
    where TValue : StronglyTypedIdBaseEntity<TValue>
{
    private readonly Type _type;

    public StronglyTypedIdConverter(Type type)
    {
        _type = type;
    }

    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string)
               || sourceType == typeof(TValue)
               || base.CanConvertFrom(context, sourceType);
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
    {
        return destinationType == typeof(string)
               || destinationType == typeof(TValue)
               || base.CanConvertTo(context, destinationType);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string s)
        {
            StronglyTypedIdBaseEntity<TValue>.TryParse(s, out var result);
            return result;
        }

        return base.ConvertFrom(context, culture, value);
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value,
        Type destinationType)
    {
        if (value is null)
        {
            return value;
        }

        var stronglyTypedId = (StronglyTypedIdBaseEntity<TValue>) value;
        var idValue = stronglyTypedId;
        if (destinationType == typeof(string))
        {
            return idValue.ToString();
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}