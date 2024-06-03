namespace PixelPilot.PixelHttpClient.Responses.Collections;

public class PlayerEntry : ICollectionEntry
{
    public string CollectionId { get; set; } = null!;
    public string CollectionName { get; set; } = null!;
    public string Created { get; set; } = null!;
    public bool Admin { get; set; }
    public bool Banned { get; set; }
    public int Face { get; set; }
    public string Id { get; set; } = null!;
    public string Username { get; set; } = null!;
}