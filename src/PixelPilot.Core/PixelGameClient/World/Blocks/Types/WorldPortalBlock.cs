using System.Drawing;
using PixelPilot.PixelGameClient.Messages;
using PixelPilot.PixelGameClient.Messages.Send;
using PixelPilot.PixelGameClient.World.Constants;

namespace PixelPilot.PixelGameClient.World.Blocks;

public class WorldPortalBlock : BasicBlock
{
    public string WorldId { get; set; }

    public WorldPortalBlock(string worldId) : base(PixelBlock.PortalWorld)
    {
        WorldId = worldId;
    }

    public override IPixelGamePacketOut AsPacketOut(int x, int y, int layer)
    {
        return new WorldBlockPlacedOutPacket(x, y, layer, BlockId, [WorldId]);
    }
    
    public override IPixelGamePacketOut AsPacketOut(List<Point> positions, int layer)
    {
        return new WorldBlockPlacedOutPacket(positions, layer, BlockId, [WorldId]);
    }
    
    public override byte[] AsWorldBuffer(int x, int y, int layer, int customId)
    {
        using MemoryStream memoryStream = new MemoryStream();
        using BinaryWriter writer = new BinaryWriter(memoryStream);
        
        writer.Write(x);
        writer.Write(y);
        writer.Write(layer);
        writer.Write(customId);
        writer.Write(WorldId);

        return memoryStream.ToArray();
    }
}