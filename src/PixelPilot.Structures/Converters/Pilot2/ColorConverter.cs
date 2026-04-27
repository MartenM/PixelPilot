using System.Drawing;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PixelPilot.Structures.Converters.Pilot2;

public class ColorConverter : JsonConverter<Color>
{
    public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var value = reader.GetString();

            if (string.IsNullOrWhiteSpace(value))
                return Color.Empty;

            // Hex format: #RRGGBB or #AARRGGBB
            if (value.StartsWith("#"))
            {
                value = value.TrimStart('#');

                if (value.Length == 6)
                {
                    // RRGGBB
                    var r = int.Parse(value.Substring(0, 2), NumberStyles.HexNumber);
                    var g = int.Parse(value.Substring(2, 2), NumberStyles.HexNumber);
                    var b = int.Parse(value.Substring(4, 2), NumberStyles.HexNumber);
                    return Color.FromArgb(r, g, b);
                }
                else if (value.Length == 8)
                {
                    // AARRGGBB
                    var a = int.Parse(value.Substring(0, 2), NumberStyles.HexNumber);
                    var r = int.Parse(value.Substring(2, 2), NumberStyles.HexNumber);
                    var g = int.Parse(value.Substring(4, 2), NumberStyles.HexNumber);
                    var b = int.Parse(value.Substring(6, 2), NumberStyles.HexNumber);
                    return Color.FromArgb(a, r, g, b);
                }

                throw new JsonException($"Invalid color hex format: {value}");
            }

            // Named color
            return Color.FromName(value);
        }

        throw new JsonException($"Unexpected token parsing Color: {reader.TokenType}");
    }

    public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
    {
        if (value.IsNamedColor)
        {
            writer.WriteStringValue(value.Name);
            return;
        }

        // Write as #AARRGGBB
        writer.WriteStringValue(
            $"#{value.A:X2}{value.R:X2}{value.G:X2}{value.B:X2}"
        );
    }
}