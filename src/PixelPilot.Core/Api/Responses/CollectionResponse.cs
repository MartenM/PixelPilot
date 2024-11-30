using PixelPilot.Api.Responses.Collections;

namespace PixelPilot.Api.Responses;

public class CollectionResponse<T> where T : ICollectionEntry
{
    public int Page { get; set; }
    public int PerPage { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public List<T> Items { get; set; } = null!;
}