namespace PixelPilot.Api.Responses;

/// <summary>
/// A single entry of a published world's leaderboard.
/// </summary>
public class LeaderboardEntry
{
    /// <summary>
    /// Rank on the leaderboard, starting at 1. Tied entries share the same index.
    /// </summary>
    public int Index { get; set; }
    public string UserId { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string Smiley { get; set; } = null!;

    /// <summary>
    /// The ranked value of this entry. Lower values rank higher.
    /// </summary>
    public int Value { get; set; }

    /// <summary>
    /// Whether a ghost replay of this run is available.
    /// </summary>
    public bool HasReplay { get; set; }
}
