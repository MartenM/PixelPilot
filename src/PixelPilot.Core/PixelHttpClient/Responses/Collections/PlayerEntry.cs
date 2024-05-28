namespace PixelPilot.PixelHttpClient.Responses.Collections;

public class PlayerEntry : ICollectionEntry
{
    public string CollectionId { get; set; }
    public string CollectionName { get; set; }
    public string Created { get; set; }
    public bool Admin { get; set; }
    public bool Banned { get; set; }
    public int Face { get; set; }
    public string Id { get; set; }
    public string Username { get; set; }
}