using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.StronglyTypedIds;

public class StronglyTypedIdJsonConverterFactory : JsonConverterFactory
{
    private static readonly ConcurrentDictionary<Type, JsonConverter> Cache = new();

    public override bool CanConvert(Type typeToConvert)
    {
        return StronglyTypedIdHelper.IsStronglyTypedId(typeToConvert);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return Cache.GetOrAdd(typeToConvert, CreateConverter);
    }

    private static JsonConverter CreateConverter(Type typeToConvert)
    {
        if (!StronglyTypedIdHelper.IsStronglyTypedId(typeToConvert))
        {
            throw new InvalidStronglyTypedIdException($"Cannot create converter for '{typeToConvert}'");
        }

        var type = typeof(StronglyTypedIdJsonConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter) Activator.CreateInstance(type)!;
    }
}

public class StronglyTypedIdJsonConverter<TStronglyTypedId> : JsonConverter<TStronglyTypedId>
    where TStronglyTypedId : StronglyTypedIdBaseEntity<TStronglyTypedId>
{
    public override TStronglyTypedId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is JsonTokenType.Null)
        {
            return null;
        }

        var value = JsonSerializer.Deserialize<string>(ref reader, options);
        if (value is not null)
        {
            StronglyTypedIdBaseEntity<TStronglyTypedId>.TryParse(value, out var result);
            return result;
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, TStronglyTypedId? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
        }
        else
        {
            JsonSerializer.Serialize(writer, value.ToString(), options);
        }
    }
}