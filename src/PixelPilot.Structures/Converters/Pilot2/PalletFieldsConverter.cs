using System.Text.Json;
using System.Text.Json.Serialization;

namespace PixelPilot.Structures.Converters.Pilot2;

public class PalletFieldsConverter : JsonConverter<Dictionary<string, object>>
{
    private const string TypeProperty = "__type";
    private const string ValueProperty = "__value";

    public override Dictionary<string, object> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("Expected JSON object.");

        var dict = new Dictionary<string, object>();

        using var doc = JsonDocument.ParseValue(ref reader);

        foreach (var prop in doc.RootElement.EnumerateObject())
        {
            var element = prop.Value;

            // Typed wrapper object
            if (element.ValueKind == JsonValueKind.Object &&
                element.TryGetProperty(TypeProperty, out var typeProp) &&
                element.TryGetProperty(ValueProperty, out var valueProp))
            {
                var typeName = typeProp.GetString();
                var targetType = Type.GetType(typeName!);

                if (targetType == null)
                    throw new JsonException($"Unknown type: {typeName}");

                object value;

                // Special-case byte[]
                if (targetType == typeof(byte[]))
                {
                    var base64 = valueProp.GetString();
                    value = Convert.FromBase64String(base64!);
                }
                else
                {
                    value = JsonSerializer.Deserialize(valueProp.GetRawText(), targetType, options)!;
                }

                dict[prop.Name] = value;
            }
            else
            {
                dict[prop.Name] = JsonElementToObject(element);
            }
        }

        return dict;
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<string, object> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        foreach (var kvp in value)
        {
            writer.WritePropertyName(kvp.Key);

            if (kvp.Value == null)
            {
                writer.WriteNullValue();
                continue;
            }

            var type = kvp.Value.GetType();

            if (IsPrimitiveSafe(type))
            {
                WriteTyped(writer, kvp.Value, type, options);
            }
            else if (type == typeof(byte[]))
            {
                WriteByteArray(writer, (byte[])kvp.Value);
            }
            else
            {
                // Non-primitive object fallback
                JsonSerializer.Serialize(writer, kvp.Value, type, options);
            }
        }

        writer.WriteEndObject();
    }

    private static void WriteTyped(Utf8JsonWriter writer, object value, Type type, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString(TypeProperty, type.AssemblyQualifiedName);

        writer.WritePropertyName(ValueProperty);
        JsonSerializer.Serialize(writer, value, type, options);

        writer.WriteEndObject();
    }

    private static void WriteByteArray(Utf8JsonWriter writer, byte[] bytes)
    {
        writer.WriteStartObject();
        writer.WriteString(TypeProperty, typeof(byte[]).AssemblyQualifiedName);
        writer.WriteString(ValueProperty, Convert.ToBase64String(bytes));
        writer.WriteEndObject();
    }

    private static bool IsPrimitiveSafe(Type type)
    {
        return type == typeof(int) ||
               type == typeof(uint) ||
               type == typeof(long) ||
               type == typeof(ulong) ||
               type == typeof(short) ||
               type == typeof(ushort) ||
               type == typeof(byte) ||
               type == typeof(sbyte) ||
               type == typeof(float) ||
               type == typeof(double) ||
               type == typeof(decimal) ||
               type == typeof(bool) ||
               type == typeof(string);
    }

    private static object? JsonElementToObject(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Number => element.TryGetInt64(out var l)
                ? l
                : element.GetDouble(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => null,
            _ => element.GetRawText()
        };
    }
}