namespace PixelPilot.Api.Responses.Collections;

/// <summary>
/// A world that has been published to the world library.
/// Note that <see cref="Id"/> is the published world id, which differs from the id of the
/// underlying world (<see cref="World"/>).
/// </summary>
public class PublishedWorldEntry : ICollectionEntry
{
    public string CollectionId { get; set; } = null!;
    public string CollectionName { get; set; } = null!;
    public string Created { get; set; } = null!;
    public string Updated { get; set; } = null!;

    /// <summary>
    /// The published world id. Use this for <see cref="PixelApiClient.GetWorldLeaderboard"/>.
    /// </summary>
    public string Id { get; set; } = null!;

    /// <summary>
    /// The id of the underlying world.
    /// </summary>
    public string World { get; set; } = null!;
    public string WorldData { get; set; } = null!;

    public int Completions { get; set; }
    public int Difficulty { get; set; }
    public string Quality { get; set; } = null!;
    public string Status { get; set; } = null!;
    public List<string> Tags { get; set; } = new();

    public string ProcessedBy { get; set; } = null!;
    public string ProcessedDate { get; set; } = null!;

    public PublishedWorldExpanded? Expand { get; set; }
}

public class PublishedWorldExpanded
{
    public WorldEntry? World { get; set; }
}
