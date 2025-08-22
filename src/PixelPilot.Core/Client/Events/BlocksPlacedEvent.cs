using System.Drawing;
using PixelPilot.Client.World.Blocks;
using PixelPilot.Client.World.Constants;

namespace PixelPilot.Client.Events;

public class BlocksPlacedEvent : EventBase
{
    /// <summary>
    /// The user that placed the blocks.
    /// </summary>
    public int UserId { get; init; }
    
    /// <summary>
    /// The block that is being placed with this packet.
    /// </summary>
    public required IPixelBlock NewBlock { get; init; }
    
    /// <summary>
    /// The on which the blocks have been placed.
    /// </summary>
    public required WorldLayer Layer { get; init; }
    
    /// <summary>
    /// The positions the new block will be placed on.
    /// </summary>
    public required IEnumerable<Point> Positions { get; init; }
}