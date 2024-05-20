using System.Text.Json;
using Microsoft.Extensions.Logging;
using PixelPilot.Common.Logging;
using PixelPilot.PixelGameClient.Messages;
using PixelPilot.PixelGameClient.Messages.Received;
using PixelPilot.PixelGameClient.World.Blocks;
using PixelPilot.PixelGameClient.World.Blocks.Placed;
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
    /// The 
    /// </summary>
    public event BlockPlaced? OnBlockPlaced;
    
    /// <summary>
    /// Represents a delegate for the BlockPlaced event.
    /// </summary>
    /// <param name="sender">The object that triggered the event.</param>
    /// <param name="userId">The ID of the user who placed the block.</param>
    /// <param name="oldBlock">The previous state of the block.</param>
    /// <param name="newBlock">The new state of the block after being placed. Includes X, Y, Layer.</param>
    public delegate void BlockPlaced(object sender, int userId, IPlacedBlock oldBlock, IPlacedBlock newBlock);
   
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
        return BlockAt((int) layer, x, y);
    }
    
    /// <summary>
    /// Gets the block at the specified point.
    /// </summary>
    /// <param name="layer">Layer</param>
    /// <param name="x">X</param>
    /// <param name="y">Y</param>
    /// <returns>The block</returns>
    public IPixelBlock BlockAt(int layer, int x, int y)
    {
        return _worldData[layer, x, y];
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

        try
        {
            // Background
            DeserializeLayer(0, reader);

            // Foreground
            DeserializeLayer(1, reader);
        }
        catch (EndOfStreamException ex)
        {
            _logger.LogError(ex, "The World reader unexpectedly reached the end of the stream. Are you sure the API is up-to-date?");
        }

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
            OnBlockPlaced?.Invoke(this, place.PlayerId, new PlacedBlock(place.X, place.Y, place.Layer, oldBlock), new PlacedBlock(place.X, place.Y, place.Layer, block));
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
                _worldData[layer, x, y] = DeserializeBlock(reader);
            }
        }
    }

    /// <summary>
    /// Desterialize a blok
    /// </summary>
    /// <param name="reader">Memory stream reader</param>
    /// <returns>The block</returns>
    /// <exception cref="ArgumentOutOfRangeException">Only when implementation is missing</exception>
    public static IPixelBlock DeserializeBlock(BinaryReader reader)
    {
        var block = (PixelBlock) reader.ReadInt32();
        
        // We want to construct a PixelBlock.
        // First we need to know what type it is.
        // Then we can fill in the rest.

        var blockType = block.GetBlockType();
        var extraFields = blockType.GetPacketFieldTypes();

        // Read the extra fields
        var extra = extraFields.Select(fieldT => PacketConverter.ReadTypeLe(reader, fieldT)).ToList();
        
        // Construct the block and return it. Hooray.
        switch (blockType)
        {
            case BlockType.Normal:
                return new BasicBlock((int) block);
            case BlockType.Morphable:
                return new MorphableBlock((int)block, extra[0]);
            case BlockType.Portal:
                return new PortalBlock( (int)block, extra[1], extra[2], extra[0]);
            case BlockType.SwitchActivator:
                return new ActivatorBlock((int) block, extra[0], extra[1]);
            case BlockType.SwitchResetter:
                return new ResetterBlock((int)block, extra[0]);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    /// <summary>
    /// Deserializes a WorldBlockPlacedPacket into an IPixelBlock object.
    /// This conversion loses information about where, or by who, a block was placed.
    /// </summary>
    /// <param name="packet">The WorldBlockPlacedPacket to deserialize.</param>
    /// <exception cref="NotImplementedException">If the type has not been implemented yet.</exception>
    /// <returns>An IPlacedBlock object representing the deserialized packet but without location data.</returns>
    public static IPixelBlock DeserializeBlock(WorldBlockPlacedPacket packet)
    {
        var pixelBlock = (PixelBlock) packet.BlockId;
        
        // We want to construct a PixelBlock.
        // First we need to know what type it is.
        // Then we can fill in the rest.
        var blockType = pixelBlock.GetBlockType();
        
        // Construct the block and return it. Hooray.
        switch (blockType)
        {
            case BlockType.Normal:
                return new BasicBlock((int) pixelBlock);
            case BlockType.Morphable:
                return new MorphableBlock((int) pixelBlock, packet.ExtraInt1!.Value);
            case BlockType.Portal:
                return new PortalBlock((int) pixelBlock, packet.ExtraInt2!.Value, packet.ExtraInt3!.Value, packet.ExtraInt1!.Value);
            case BlockType.SwitchActivator:
                return new ActivatorBlock((int) pixelBlock, packet.ExtraInt1!.Value, packet.ExtraByte!.Value == 1);
            case BlockType.SwitchResetter:
                return new ResetterBlock((int)pixelBlock, packet.ExtraByte!.Value == 1);
            default:
                throw new NotImplementedException("Missing implementation of new BlockType!");
        }
    }

    /// <summary>
    /// Deserializes a WorldBlockPlacedPacket into an IPlacedBlock object.
    /// </summary>
    /// <param name="packet">The WorldBlockPlacedPacket to deserialize.</param>
    /// <exception cref="NotImplementedException">If the type has not been implemented yet.</exception>
    /// <returns>An IPlacedBlock object representing the deserialized packet.</returns>
    public static IPlacedBlock DeserializePlacedBlock(WorldBlockPlacedPacket packet)
    {
        var pixelBlock = (PixelBlock) packet.BlockId;
        
        // We want to construct a PixelBlock.
        // First we need to know what type it is.
        // Then we can fill in the rest.
        var blockType = pixelBlock.GetBlockType();
        
        // Construct the block and return it. Hooray.
        IPixelBlock block;
        switch (blockType)
        {
            case BlockType.Normal:
                block = new BasicBlock((int) pixelBlock);
                break;
            case BlockType.Morphable:
                block = new MorphableBlock((int) pixelBlock, packet.ExtraInt1!.Value);
                break;
            case BlockType.Portal:
                block = new PortalBlock((int) pixelBlock, packet.ExtraInt2!.Value, packet.ExtraInt3!.Value, packet.ExtraInt1!.Value);
                break;
            case BlockType.SwitchActivator:
                block = new ActivatorBlock((int) pixelBlock, packet.ExtraInt1!.Value, packet.ExtraByte!.Value == 1);
                break;
            case BlockType.SwitchResetter:
                block = new ResetterBlock((int)pixelBlock, packet.ExtraByte!.Value == 1);
                break;
            default:
                throw new NotImplementedException("Missing implementation of new BlockType!");
        }

        return new PlacedBlock(packet.X, packet.Y, packet.Layer, block);
    }
}