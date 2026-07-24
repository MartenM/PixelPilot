using System.Text.Json;
using System.Text.Json.Serialization;
using Google.Protobuf;
using PixelPilot.Client.World.Zones;

namespace PixelPilot.Structures.Converters.Pilot2;

/// <summary>
/// Serializes a zone's membership grid as its own width/height plus a base64 RLE payload (see
/// <see cref="MembershipRle"/> for the wire format), so it can be decoded independently of
/// property order/whether the owning Zone's Width/Height fields have been read yet.
/// </summary>
public class ZoneMembershipConverter : JsonConverter<bool[,]>
{
    public override bool[,] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException($"Unexpected token parsing zone membership: {reader.TokenType}");

        var width = 0;
        var height = 0;
        var rle = "";

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject) break;
            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException("Expected property name parsing zone membership.");

            var propertyName = reader.GetString();
            reader.Read();

            switch (propertyName)
            {
                case "width":
                    width = reader.GetInt32();
                    break;
                case "height":
                    height = reader.GetInt32();
                    break;
                case "rle":
                    rle = reader.GetString() ?? "";
                    break;
            }
        }

        var bytes = string.IsNullOrEmpty(rle) ? ByteString.Empty : ByteString.CopyFrom(Convert.FromBase64String(rle));
        return MembershipRle.Decode(bytes, width, height);
    }

    public override void Write(Utf8JsonWriter writer, bool[,] value, JsonSerializerOptions options)
    {
        var width = value.GetLength(0);
        var height = value.GetLength(1);
        var rle = Convert.ToBase64String(MembershipRle.Encode(value).ToByteArray());

        writer.WriteStartObject();
        writer.WriteNumber("width", width);
        writer.WriteNumber("height", height);
        writer.WriteString("rle", rle);
        writer.WriteEndObject();
    }
}
