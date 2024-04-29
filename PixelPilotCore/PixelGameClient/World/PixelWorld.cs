using System.Text.Json;
using Microsoft.Extensions.Logging;
using PixelPilot.PixelGameClient.Messages;
using PixelPilot.PixelGameClient.Messages.Received;
using PixelPilot.PixelGameClient.World.Blocks;
using PixelPilot.PixelGameClient.World.Constants;

namespace PixelPilot.PixelGameClient.World;

/// <summary>
/// Represents the 'world' in PixelWalker.
/// The world includes blocks, switches and other things that can be placed or interacted with.
/// This does not include players!
/// </summary>
public class PixelWorld
{
    private static ILogger _logger = LogManager.GetLogger("PixelPilot.World");
    public int Height { get; private set; }
    public int Width { get; private set; }

    private IPixelBlock[,,] _worldData;
    
    /// <summary>
    /// Fired once init has been received by the client.
    /// </summary>
    public event BlockPlaced? OnBlockPlaced;
    public delegate void BlockPlaced(object sender, int userId, IPixelBlock oldBlock, IPixelBlock newBlock);
   
    public PixelWorld()
    {
        _worldData = new IPixelBlock[2, 0, 0];
    }
    public PixelWorld(int height, int width)
    {
        Height = height;
        Width = width;
        _worldData = new IPixelBlock[2, width, height];
    }

    public PixelWorld(InitPacket initPacket) : this(initPacket.Height, initPacket.Width)
    {
        Init(initPacket.WorldData);
    }

    /// <summary>
    /// Gets the block at the specified point.
    /// </summary>
    /// <param name="layer">Layer</param>
    /// <param name="x">X</param>
    /// <param name="y">Y</param>
    /// <returns>The block</returns>
    public IPixelBlock BlockAt(WorldLayer layer, int x, int y)
    {
        return _worldData[(int)layer, x, y];
    }
    
    /// <summary>
    /// Initialize the world using a byte[].
    /// </summary>
    /// <param name="buffer"></param>
    /// <exception cref="Exception"></exception>
    public void Init(byte[] buffer)
    {
        if (Width == 0 || Height == 0) throw new Exception("World with and height must be set before serializing data!");
        
        using var stream = new MemoryStream(buffer);
        using var reader = new BinaryReader(stream);
        
        // Background
        DeserializeLayer(0, reader);
        
        // Foreground
        DeserializeLayer(1, reader);

        if (stream.Position != stream.Length)
            throw new Exception(
                $"Error while converting world buffer. Reader is at {stream.Position} while lenght is {stream.Length}");
    }

    /// <summary>
    /// Utility method that can attached to the client.
    /// This allows for an easy hook without having to write this each time.
    /// </summary>
    /// <param name="sender">The sender</param>
    /// <param name="packet">The incoming packet</param>
    public void HandlePacket(Object sender, IPixelGamePacket packet)
    {
        if (packet is InitPacket init)
        {
            Height = init.Height;
            Width = init.Width;
            _worldData = new IPixelBlock[2, Width, Height];
            Init(init.WorldData);
            return;
        }

        if (packet is WorldReloadedPacket reload)
        {
            Init(reload.WorldData);
            return;
        }

        if (packet is WorldBlockPlacedPacket place)
        {
            var block = DeserializeBlock(place);
            var oldBlock = _worldData[place.Layer, place.X, place.Y];
            _worldData[place.Layer, place.X, place.Y] = block;
            OnBlockPlaced?.Invoke(this, place.PlayerID, oldBlock, block);
        }
    }
    
    /// <summary>
    /// Deserialize a layer.
    /// </summary>
    /// <param name="layer">Current layer</param>
    /// <param name="reader">Memory stream reader</param>
    private void DeserializeLayer(int layer, BinaryReader reader)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                _worldData[layer, x, y] = DeserializeBlock(x, y, layer, reader);
            }
        }
    }

    /// <summary>
    /// Desterialize a blok
    /// </summary>
    /// <param name="x">Current X</param>
    /// <param name="y">Current Y</param>
    /// <param name="layer">Current Layer</param>
    /// <param name="reader">Memory stream reader</param>
    /// <returns>The block</returns>
    /// <exception cref="ArgumentOutOfRangeException">Only when implementation is missing</exception>
    private IPixelBlock DeserializeBlock(int x, int y, int layer, BinaryReader reader)
    {
        var block = (PixelBlock) reader.ReadInt32();
        
        // We want to construct a PixelBlock.
        // First we need to know what type it is.
        // Then we can fill in the rest.

        var blockType = block.GetBlockType();
        var extraFields = blockType.GetPacketFieldTypes();

        // Read the extra fields
        var extra = extraFields.Select(fieldT => PacketConverter.ReadType(reader, fieldT)).ToList();
        
        // Construct the block and return it. Hooray.
        switch (blockType)
        {
            case BlockType.Normal:
                return new BasicBlock(x, y, layer, (int) block);
            case BlockType.Morphable:
                return new MorphableBlock(x, y, layer, (int)block, extra[0]);
            case BlockType.Portal:
                return new PortalBlock(x, y, layer, (int)block, extra[2], extra[1], extra[0]);
            case BlockType.SwitchActivator:
                return new ActivatorBlock(x, y, layer, (int) block, extra[0], extra[1]);
            case BlockType.SwitchResetter:
                return new ResetterBlock(x, y, layer, (int)block, extra[0]);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private IPixelBlock DeserializeBlock(WorldBlockPlacedPacket packet)
    {
        var block = (PixelBlock) packet.BlockId;
        
        // We want to construct a PixelBlock.
        // First we need to know what type it is.
        // Then we can fill in the rest.

        var blockType = block.GetBlockType();
        var extraFields = blockType.GetPacketFieldTypes();
        
        // Construct the block and return it. Hooray.
        switch (blockType)
        {
            case BlockType.Normal:
                return new BasicBlock(packet.X, packet.Y, packet.Layer, (int) block);
            case BlockType.Morphable:
                return new MorphableBlock(packet.X, packet.Y, packet.Layer, (int)block, packet.ExtraInt1!.Value);
            case BlockType.Portal:
                return new PortalBlock(packet.X, packet.Y, packet.Layer, (int)block, packet.ExtraInt3!.Value, packet.ExtraInt2!.Value, packet.ExtraInt1!.Value);
            case BlockType.SwitchActivator:
                return new ActivatorBlock(packet.X, packet.Y, packet.Layer, (int) block, packet.ExtraInt1!.Value, packet.ExtraByte!.Value == 1);
            case BlockType.SwitchResetter:
                return new ResetterBlock(packet.X, packet.Y, packet.Layer, (int)block, packet.ExtraByte!.Value == 1);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}