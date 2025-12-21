namespace PixelPilot.Api.Responses;

public class PixelBlockMeta
{
    public required int Id { get; init; }
    public required string PaletteId { get; init; }
    public required int Layer { get; init; }
    
    public long? MinimapColor { get; init; }
    public int? LegacyId { get; init; }

    public List<FieldData> Fields { get; set; } = new();
}