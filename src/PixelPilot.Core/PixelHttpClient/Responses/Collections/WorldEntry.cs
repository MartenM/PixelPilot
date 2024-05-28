namespace PixelPilot.PixelHttpClient.Responses.Collections;

public class WorldEntry : ICollectionEntry
{
    public string CollectionId { get; set; } = null!;
    public string CollectionName { get; set; } = null!;
    public string Created { get; set; } = null!;
    public string Data { get; set; } = null!;
    public int Height { get; set; }
    public int Width { get; set; }
    public string Id { get; set; } = null!;
    public int Plays { get; set; }
    public string Title { get; set; } = null!;
    public string Updated { get; set; } = null!;
    public string Visibility { get; set; } = null!;
}