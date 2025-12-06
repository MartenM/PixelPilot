using System.Drawing;
using System.Text.RegularExpressions;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Microsoft.Extensions.Logging;
using PixelPilot.Api;
using PixelPilot.Client.Abstract;
using PixelPilot.Client.Events;
using PixelPilot.Client.Extensions;
using PixelPilot.Client.Messages;
using PixelPilot.Client.World.Blocks;
using PixelPilot.Client.World.Blocks.Placed;
using PixelPilot.Client.World.Blocks.Types;
using PixelPilot.Client.World.Blocks.Types.Effects;
using PixelPilot.Client.World.Blocks.Types.Music;
using PixelPilot.Client.World.Blocks.V2;
using PixelPilot.Client.World.Constants;
using PixelPilot.Common.Logging;
using PixelWalker.Networking.Protobuf.WorldPackets;

namespace PixelPilot.Client.World;

/// <summary>
/// Represents the 'world' in PixelWalker.
/// The world includes blocks, switches and other things that can be placed or interacted with.
/// This does not include players!
/// </summary>
public class PixelWorld
{
    private static ILogger _logger = LogManager.GetLogger("PixelPilot.World");
    private readonly TaskCompletionSource<bool> _initializationTaskSource = new();
    private IPixelPilotClient _client;
    
    /// <summary>
    /// A task that can be used to await world init completion.
    /// This ensures the world has been properly populated before using it.
    /// </summary>
    public Task InitTask => _initializationTaskSource.Task;
    public int Height { get; private set; }
    public int Width { get; private set; }

    private WorldMeta? _worldMeta { get; set; }
    public string OwnerUsername => _worldMeta?.Owner ?? string.Empty;
    public string WorldName => _worldMeta?.Title ?? string.Empty;

    private IPixelBlock[,,] _worldData;
    
    /// <summary>
    /// Fired when a block was placed.
    /// </summary>
    [Obsolete("Please use OnBlocksPlaced instead.")]
    public event BlockPlaced? OnBlockPlaced;
    
    /// <summary>
    /// Represents a delegate for the BlockPlaced event.
    /// </summary>
    /// <param name="sender">The object that triggered the event.</param>
    /// <param name="userId">The ID of the user who placed the block.</param>
    /// <param name="oldBlock">The previous state of the block.</param>
    /// <param name="newBlock">The new state of the block after being placed. Includes X, Y, Layer.</param>
    public delegate void BlockPlaced(object sender, int userId, IPlacedBlock oldBlock, IPlacedBlock newBlock);
    
    /// <summary>
    /// Fired when blocks have been placed.
    /// </summary>
    public event BlocksPlaced? OnBlocksPlaced;
    public delegate void BlocksPlaced(object sender, BlocksPlacedEvent blocksEvent);
    
    /// <summary>
    /// Fired after the world is initialized.
    /// </summary>
    public event WorldInit? OnWorldInit;
    
    /// <summary>
    /// Represents a delegate for the WorldInit event.
    /// </summary>
    /// <param name="sender">The object that triggered the event.</param>
    public delegate void WorldInit(object sender);
    
    /// <summary>
    /// Fired after the world is reloaded
    /// </summary>
    public event WorldReloaded? OnWorldReloaded;
    
    /// <summary>
    /// Represents a delegate for the WorldReloaded event.
    /// </summary>
    /// <param name="sender">The object that triggered the event.</param>
    public delegate void WorldReloaded(object sender);
    
    /// <summary>
    /// Fired after the world is initialized.
    /// </summary>
    public event WorldCleared? OnWorldCleared;
    
    /// <summary>
    /// Represents a delegate for the WorldCleared event.
    /// </summary>
    /// <param name="sender">The object that triggered the event.</param>
    public delegate void WorldCleared(object sender);

    public PixelWorld(IPixelPilotClient client)
    {
        _client = client;
        _worldData = new IPixelBlock[3, 0, 0];
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
    [Obsolete]
    public void LegacyInit(byte[] buffer)
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
            
            // Overlay layer
            DeserializeLayer(2, reader);
        }
        catch (EndOfStreamException ex)
        {
            _logger.LogError(ex, "The World reader unexpectedly reached the end of the stream. Are you sure the API is up-to-date?");
        }

        if (stream.Position != stream.Length)
            throw new Exception(
                $"Error while converting world buffer. Reader is at {stream.Position} while length is {stream.Length}");
    }

    /// <summary>
    /// Utility method that can attached to the client.
    /// This allows for an easy hook without having to write this each time.
    /// </summary>
    /// <param name="sender">The sender</param>
    /// <param name="packet">The incoming packet</param>
    public void HandlePacket(Object sender, IMessage packet)
    {
        if (packet is PlayerInitPacket init)
        {
            Height = init.WorldHeight;
            Width = init.WorldWidth;
            _worldMeta = init.WorldMeta;
            _worldData = new IPixelBlock[3, Width, Height];
            
            HandleWorldReload(new WorldBlockData
            {
                Pallet = init.BlockDataPalette.ToList(),
                BackgroundData = init.BackgroundLayerData.ToByteArray(),
                ForegroundData = init.ForegroundLayerData.ToByteArray(),
                OverlayData = init.OverlayLayerData.ToByteArray(),
            });
            
            OnWorldInit?.Invoke(this);
            _initializationTaskSource.TrySetResult(true);
            return;
        }

        if (packet is WorldMetaUpdatePacket meta)
        {
            _worldMeta = meta.Meta;
        }

        if (packet is WorldReloadedPacket reload)
        {
            
            OnWorldReloaded?.Invoke(this);
            return;
        }
        
        if (packet is WorldClearedPacket clear)
        {
            _worldData = new IPixelBlock[3, Width, Height];
            for (int l = 0; l < 3; l++)
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        if (l == 1 && x == 0 || x == Width - 1 || y == 0 || y == Height - 1)
                        {
                            _worldData[l, x, y] = new FlexBlock(PixelBlock.BasicGray);
                        }
                        else
                        {
                            _worldData[l, x, y] = new FlexBlock(PixelBlock.Empty);
                        }
                    }
                }
            }
            OnWorldCleared?.Invoke(this);
            return;
        }

        if (packet is WorldBlockPlacedPacket place)
        {
            var blockPlacedEvent = new BlocksPlacedEvent()
            {
                UserId = place.PlayerId,
                NewBlock = new FlexBlock(place.BlockId, ToPixelFieldDict(place.Fields)),
                Layer = (WorldLayer) place.Layer,
                Positions = place.Positions.Select(p => new Point(p.X, p.Y))
            };
            OnBlocksPlaced?.Invoke(this, blockPlacedEvent);
            
            // If cancelled revert all blocks.
            if (blockPlacedEvent.Cancelled)
            {
                var revertBlocks = place.Positions
                    .Select(p => new PlacedBlock(p.X, p.Y, place.Layer, _worldData[place.Layer, p.X, p.Y]))
                    .ToList();
                
                _client.SendRange(revertBlocks.ToChunkedPackets());
                return;
            }
            
            // Otherwise update the world map.
            foreach (var point in place.Positions)
            {
                // Rather make a copy of this, BUT that's currently not possible.
                // TODO: Deep clone blocks
                var block = new FlexBlock(place.BlockId, ToPixelFieldDict(place.Fields));
                var oldBlock = _worldData[place.Layer, point.X, point.Y];
                
                _worldData[place.Layer, point.X, point.Y] = block;
                OnBlockPlaced?.Invoke(this, place.PlayerId, new PlacedBlock(point.X, point.Y, place.Layer, oldBlock), new PlacedBlock(point.X, point.Y, place.Layer, block));
            }
        }
    }

    private class WorldBlockData
    {
        public required List<BlockDataInfo> Pallet { get; set; }
        public required byte[] BackgroundData { get; set; }
        public required byte[] ForegroundData { get; set; }
        public required byte[] OverlayData { get; set; }
    }

    private void HandleWorldReload(WorldBlockData worldBlockData)
    {
        _worldData = new IPixelBlock[3, Width, Height];
        
        var pallet = worldBlockData.Pallet;
        HandleLayer(pallet, worldBlockData.BackgroundData, (int) WorldLayer.Background);
        HandleLayer(pallet, worldBlockData.ForegroundData, (int) WorldLayer.Foreground);
        HandleLayer(pallet, worldBlockData.OverlayData, (int) WorldLayer.Overlay);
    }

    private void HandleLayer(List<BlockDataInfo> pallet, byte[] layerData, int layer)
    {
        var binaryStream = new MemoryStream(layerData);
        var binaryReader = new BinaryReader(binaryStream);
        
        int i = 0;
        while (i < Height * Width)
        {
            int palletId = binaryReader.Read7BitEncodedInt();
            var palletBlock = pallet[palletId];
            
            int amount = binaryReader.Read7BitEncodedInt();
            for (int di = 0; di < amount; di++) {
                int x = i / Height;
                int y = i % Height;

                _worldData[layer, x, y] = ToFlexBlock(palletBlock);
                
                i++;
            }
        }

        if (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
        {
            throw new PixelApiException($"Layer {layer} did not finish serializing yet. Are you sure the API is up-to-date?");
        }
    }

    private FlexBlock ToFlexBlock(BlockDataInfo block)
    {
        return new FlexBlock(block.BlockId, ToPixelFieldDict(block.Fields));
    }

    private Dictionary<string, object> ToPixelFieldDict(
        MapField<string, BlockFieldValue> raw)
    {
        var dict = new Dictionary<string, object>();
        foreach (var pair in raw)
        {
            dict.Add(pair.Key, FlexBlock.ToObject(pair.Value));
        }

        return dict;
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
                _worldData[layer, x, y] = Legacy_DeserializeBlock(reader);
            }
        }
    }

    public IEnumerable<PlacedBlock> GetBlocks(bool includeEmpty = true)
    {
        for (int layer = 0; layer < 2; layer++)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var block = BlockAt(layer, x, y);
                    if (!includeEmpty && block.Block == PixelBlock.Empty) continue;
                    
                    yield return new PlacedBlock(x, y, layer, block);
                }
            }
        }
    }
    
    /// <summary>
    /// Deserialize a block
    /// </summary>
    /// <param name="reader">Memory stream reader</param>
    /// <returns>The block</returns>
    /// <exception cref="ArgumentOutOfRangeException">Only when implementation is missing</exception>
    public static IPixelBlock Legacy_DeserializeBlock(BinaryReader reader)
    {
        var block = (PixelBlock) reader.ReadInt32();
        return Legacy_DeserializeBlock(reader, block);
    }
    
    public static IPixelBlock Legacy_DeserializeBlock(BinaryReader reader, PixelBlock block)
    {
        // We want to construct a PixelBlock.
        // First we need to know what type it is.
        // Then we can fill in the rest.
        var blockType = block.GetBlockType();
        var extraFields = blockType.GetPacketFieldTypes();

        // Read the extra fields
        var extra = extraFields.Select(fieldT => BinaryFieldConverter.ReadTypeLe(reader, fieldT)).ToList();
        
        // Construct the block and return it. Hooray.
        switch (blockType)
        {
            case BlockType.Normal:
                return new BasicBlock((int) block);
            case BlockType.Morphable:
                return new MorphableBlock((int)block, extra[0]);
            case BlockType.Portal:
                return new PortalBlock( (int)block, extra[0], extra[1]);
            case BlockType.SwitchActivator:
                return new ActivatorBlock((int) block, extra[0], extra[1]);
            case BlockType.SwitchResetter:
                return new ResetterBlock((int)block, extra[0]);
            case BlockType.WorldPortal:
                return new WorldPortalBlock(extra[0], extra[1]);
            case BlockType.EffectLeveled:
                return new LeveledEffectBlock((int)block, extra[0]);
            case BlockType.EffectTimed:
                return new TimedEffectBlock((int)block, extra[0]);
            case BlockType.EffectTogglable:
                return new ToggleEffectBlock((int)block, extra[0]);
            case BlockType.Sign:
                return new SignBlock((int)block, extra[0]);
            case BlockType.NoteBlock:
                return new NoteBlock(block, extra[0]);
            case BlockType.ColorBlock:
            {
                var color = Color.FromArgb(
                    (int)((extra[0]) >> 24 & 0xFF), // Alpha
                    (int)((extra[0] >> 16) & 0xFF), // Red
                    (int)((extra[0] >> 8) & 0xFF), // Green
                    (int)(extra[0] & 0xFF) // Blue
                );
                return new ColoredBlock(block, color);
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
