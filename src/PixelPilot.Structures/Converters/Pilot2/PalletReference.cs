using System.Text.Json;
using System.Text.Json.Serialization;

namespace PixelPilot.Structures.Converters.Pilot2;

public class PalletReferenceConverter : JsonConverter<PalletReference>
{
    public override PalletReference? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException();

        reader.Read();
        int layer = reader.GetInt32();
        reader.Read();
        int x = reader.GetInt32();
        reader.Read();
        int y = reader.GetInt32();
        reader.Read();
        int palletIndex = reader.GetInt32();
        reader.Read();

        if (reader.TokenType != JsonTokenType.EndArray)
            throw new JsonException();

        return new PalletReference
        {
            Layer = layer,
            X = x,
            Y = y,
            PalletIndex = palletIndex
        };
    }

    public override void Write(Utf8JsonWriter writer, PalletReference value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        writer.WriteNumberValue(value.Layer);
        writer.WriteNumberValue(value.X);
        writer.WriteNumberValue(value.Y);
        writer.WriteNumberValue(value.PalletIndex);
        writer.WriteEndArray();
    }
}