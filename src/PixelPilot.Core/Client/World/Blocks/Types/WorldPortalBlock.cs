using System.Drawing;
using PixelPilot.Client.Messages;
using PixelPilot.Client.Messages.Packets.Extensions;
using PixelPilot.Client.World.Constants;
using PixelWalker.Networking.Protobuf.WorldPackets;

namespace PixelPilot.Client.World.Blocks.Types;

public class WorldPortalBlock : BasicBlock
{
    public string WorldId { get; set; }
    public int SpawnId { get; set; }
    
    public WorldPortalBlock(string worldId, int spawnId) : base(PixelBlock.PortalWorld)
    {
        WorldId = worldId;
        SpawnId = spawnId;
    }

    public override WorldBlockPlacedPacket AsPacketOut(int x, int y, int layer)
    {
        return WorldBlockPacketBuilder.CreatePacket(x, y, layer, BlockId, [WorldId, SpawnId]);
    }
    
    public override WorldBlockPlacedPacket AsPacketOut(List<Point> positions, int layer)
    {
        return WorldBlockPacketBuilder.CreatePacket(positions, layer, BlockId, [WorldId, SpawnId]);
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
        writer.Write(SpawnId);

        return memoryStream.ToArray();
    }
}