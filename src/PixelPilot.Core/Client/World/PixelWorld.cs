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
using PixelPilot.Client.Messages.Packets.Extensions;
using PixelPilot.Client.World.Blocks;
using PixelPilot.Client.World.Blocks.Placed;
using PixelPilot.Client.World.Blocks.Types;
using PixelPilot.Client.World.Blocks.Types.Effects;
using PixelPilot.Client.World.Blocks.Types.Music;
using PixelPilot.Client.World.Blocks.V2;
using PixelPilot.Client.World.Constants;
using PixelPilot.Client.World.Labels;
using PixelPilot.Client.World.Meta;
using PixelPilot.Client.World.Zones;
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
    
    public int Height { get; private set; }
    public int Width { get; private set; }

    public PixelWorldSettings WorldSettings => new PixelWorldSettings(_worldMeta!);
    
    private WorldMeta? _worldMeta;
    public WorldMeta InternalWorldMeta => _worldMeta ?? throw new PixelGameException("World is not ready yet.");
    public string OwnerUsername => _worldMeta?.Owner ?? string.Empty;
    public string WorldName => _worldMeta?.Title ?? string.Empty;

    private IPixelBlock[,,] _worldData;

    private readonly Dictionary<string, TextLabel> _labels;

    private readonly Dictionary<string, Zone> _zones;
    private readonly List<string> _zoneOrder;
    
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

    /// <summary>
    /// Fired when a zone is created or its settings change. Does not fire for membership-only
    /// changes applied via area-edit packets.
    /// </summary>
    public event ZoneUpserted? OnZoneUpserted;

    /// <summary>
    /// Represents a delegate for the ZoneUpserted event.
    /// </summary>
    /// <param name="sender">The object that triggered the event.</param>
    /// <param name="zone">The zone as it is after the upsert, including its (possibly newly assigned) id.</param>
    public delegate void ZoneUpserted(object sender, IPlacedZone zone);

    public PixelWorld(IPixelPilotClient client)
    {
        _client = client;
        _worldData = new IPixelBlock[3, 0, 0];
        _labels = new Dictionary<string, TextLabel>();
        _zones = new Dictionary<string, Zone>();
        _zoneOrder = new List<string>();
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
    /// Get all labels in the world.
    /// </summary>
    /// <returns></returns>
    public List<IPlacedTextLabel> GetLabels()
    {
        return _labels.Select(kvp => new PlacedTextLabel()
        {
            Id = kvp.Key,
            Label = kvp.Value
        }).Cast<IPlacedTextLabel>().ToList();
    }
    
    /// <summary>
    /// Get a existing text label.
    /// </summary>
    /// <param name="labelId"></param>
    /// <returns></returns>
    public IPlacedTextLabel? GetLabel(string labelId)
    {
        if (_labels.TryGetValue(labelId, out var textLabel))
        {
            return new PlacedTextLabel()
            {
                Id = labelId,
                Label = textLabel
            };
        }

        return null;
    }

    /// <summary>
    /// Get all zones in the world, in the order maintained by the server
    /// (as delivered on load and updated by zone-reorder packets).
    /// </summary>
    /// <returns></returns>
    public List<IPlacedZone> GetZones()
    {
        return _zoneOrder
            .Where(id => _zones.ContainsKey(id))
            .Select(id => new PlacedZone()
            {
                Id = id,
                Zone = _zones[id]
            })
            .Cast<IPlacedZone>()
            .ToList();
    }

    /// <summary>
    /// Get an existing zone.
    /// </summary>
    /// <param name="zoneId"></param>
    /// <returns></returns>
    public IPlacedZone? GetZone(string zoneId)
    {
        if (_zones.TryGetValue(zoneId, out var zone))
        {
            return new PlacedZone()
            {
                Id = zoneId,
                Zone = zone
            };
        }

        return null;
    }

    /// <summary>
    /// Waits for the next <see cref="OnZoneUpserted"/> event matching <paramref name="predicate"/>.
    /// Meant to learn a newly created zone's server-assigned id: subscribe by calling this
    /// *before* sending the create request (so the echo can't be missed), await the returned
    /// task, then send it alongside the request.
    /// </summary>
    /// <param name="predicate">Matched against the zone as it is right after the upsert.</param>
    /// <param name="timeout">How long to wait before giving up.</param>
    /// <exception cref="PixelGameException">Thrown if no matching zone upsert arrives in time.</exception>
    public Task<IPlacedZone> WaitForZoneUpsert(Func<IZone, bool> predicate, TimeSpan timeout)
    {
        var tcs = new TaskCompletionSource<IPlacedZone>(TaskCreationOptions.RunContinuationsAsynchronously);

        ZoneUpserted handler = null!;
        handler = (_, zone) =>
        {
            if (!predicate(zone.Zone)) return;
            if (tcs.TrySetResult(zone))
            {
                OnZoneUpserted -= handler;
            }
        };

        OnZoneUpserted += handler;

        Task.Delay(timeout).ContinueWith(_ =>
        {
            if (tcs.TrySetException(new PixelGameException("Timed out waiting for a matching zone upsert.")))
            {
                OnZoneUpserted -= handler;
            }
        });

        return tcs.Task;
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
                TextLabels = init.TextLabels.ToList(),
                Zones = init.Zones.ToList(),
            });
            
            OnWorldInit?.Invoke(this);
            _initializationTaskSource.TrySetResult(true);
            return;
        }

        if (packet is WorldMetaUpdatePacket meta)
        {
            _worldMeta = meta.Meta;
            return;
        }

        if (packet is WorldLabelUpsertPacket textLabelUpsert)
        {
            HandleTextLabelUpsert(textLabelUpsert);
            return;
        }

        if (packet is WorldLabelDeletePacket textLabelDelete)
        {
            HandleTextLabelDelete(textLabelDelete);
            return;
        }

        if (packet is WorldZoneUpsertPacket zoneUpsert)
        {
            HandleZoneUpsert(zoneUpsert);
            return;
        }

        if (packet is WorldZoneDeletePacket zoneDelete)
        {
            HandleZoneDelete(zoneDelete);
            return;
        }

        if (packet is WorldZoneAreaEditPacket zoneAreaEdit)
        {
            HandleZoneAreaEdit(zoneAreaEdit);
            return;
        }

        if (packet is WorldZoneReorderPacket zoneReorder)
        {
            HandleZoneReorder(zoneReorder);
            return;
        }

        if (packet is WorldReloadedPacket reload)
        {
            HandleWorldReload(new WorldBlockData
            {
                Pallet = reload.BlockDataPalette.ToList(),
                BackgroundData = reload.BackgroundLayerData.ToByteArray(),
                ForegroundData = reload.ForegroundLayerData.ToByteArray(),
                OverlayData = reload.OverlayLayerData.ToByteArray(),
                TextLabels = reload.TextLabels.ToList(),
                Zones = reload.Zones.ToList(),
            });
            OnWorldReloaded?.Invoke(this);
            return;
        }
        
        if (packet is WorldClearedPacket clear)
        {
            _worldData = new IPixelBlock[3, Width, Height];
            _labels.Clear();
            _zones.Clear();
            _zoneOrder.Clear();
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
        
        public required List<ProtoTextLabel> TextLabels { get; set; }
        public required List<ProtoZone> Zones { get; set; }
    }

    private void HandleWorldReload(WorldBlockData worldBlockData)
    {
        _worldData = new IPixelBlock[3, Width, Height];
        _labels.Clear();
        _zones.Clear();
        _zoneOrder.Clear();

        var pallet = worldBlockData.Pallet;
        SerializeLayer(pallet, worldBlockData.BackgroundData, (int) WorldLayer.Background);
        SerializeLayer(pallet, worldBlockData.ForegroundData, (int) WorldLayer.Foreground);
        SerializeLayer(pallet, worldBlockData.OverlayData, (int) WorldLayer.Overlay);

        SerializeTextLabels(worldBlockData.TextLabels);
        SerializeZones(worldBlockData.Zones);
    }

    private void HandleTextLabelUpsert(WorldLabelUpsertPacket packet)
    {
        if (_labels.TryGetValue(packet.Label.Id, out var label))
        {
            label.UpdateWithProtoTextLabel(packet.Label);
            return;
        }
        
        _labels.Add(packet.Label.Id, TextLabel.FromProtoTextLabel(packet.Label));
    }
    
    private void HandleTextLabelDelete(WorldLabelDeletePacket packet)
    {
        _labels.Remove(packet.Id);
    }

    private void SerializeTextLabels(List<ProtoTextLabel> textLabels)
    {
        foreach (var label in textLabels)
        {
            _labels.Add(label.Id, TextLabel.FromProtoTextLabel(label));
        }
    }

    private void HandleZoneUpsert(WorldZoneUpsertPacket packet)
    {
        if (_zones.TryGetValue(packet.Zone.Id, out var zone))
        {
            zone.UpdateWithProtoZone(packet.Zone);
        }
        else
        {
            zone = Zone.FromProtoZone(packet.Zone);
            _zones.Add(packet.Zone.Id, zone);
            _zoneOrder.Add(packet.Zone.Id);
        }

        OnZoneUpserted?.Invoke(this, new PlacedZone { Id = packet.Zone.Id, Zone = zone });
    }

    private void HandleZoneDelete(WorldZoneDeletePacket packet)
    {
        _zones.Remove(packet.Id);
        _zoneOrder.Remove(packet.Id);
    }

    private void HandleZoneAreaEdit(WorldZoneAreaEditPacket packet)
    {
        if (!_zones.TryGetValue(packet.ZoneId, out var zone)) return;

        for (var x = packet.X; x < packet.X + packet.Width; x++)
        {
            for (var y = packet.Y; y < packet.Y + packet.Height; y++)
            {
                if (x < 0 || y < 0 || x >= zone.Width || y >= zone.Height) continue;
                zone.Membership[x, y] = packet.Add;
            }
        }
    }

    private void HandleZoneReorder(WorldZoneReorderPacket packet)
    {
        if (!_zoneOrder.Remove(packet.Id)) return;

        var index = Math.Clamp(packet.Index, 0, _zoneOrder.Count);
        _zoneOrder.Insert(index, packet.Id);
    }

    private void SerializeZones(List<ProtoZone> zones)
    {
        foreach (var zone in zones)
        {
            _zones.Add(zone.Id, Zone.FromProtoZone(zone));
            _zoneOrder.Add(zone.Id);
        }
    }

    private void SerializeLayer(List<BlockDataInfo> pallet, byte[] layerData, int layer)
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
}
