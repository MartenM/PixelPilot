namespace PixelPilot.PixelHttpClient.Responses.Collections;

public interface ICollectionEntry
{
    public string CollectionId { get; set; }
    public string CollectionName { get; set; }
    public string Created { get; set; }
}