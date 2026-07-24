using System.Drawing;
using Google.Protobuf;
using PixelPilot.Client;
using PixelPilot.Client.Extensions;
using PixelPilot.Client.World;
using PixelPilot.Client.World.Blocks;
using PixelPilot.Client.World.Blocks.Placed;
using PixelPilot.Client.World.Constants;
using PixelPilot.Client.World.Labels;
using PixelPilot.Client.World.Zones;
using PixelWalker.Networking.Protobuf.WorldPackets;

namespace PixelPilot.Structures.Extensions;

public static class WorldExtensions
{
    
    /// <summary>
    /// Get a structure from the world. If the whole world is specified labels outside the world
    /// will be saved too.
    /// </summary>
    /// <param name="world"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="copyEmpty"></param>
    /// <returns></returns>
    public static Structure GetStructure(this PixelWorld world, int x, int y, int width, int height, bool copyEmpty = true)
    {
        List<IPlacedBlock> blocks = new();
        
        // Copy the world based on X Y
        for (int layer = 2; layer >= 0; layer--)
        {
            for (int dx = 0; dx < width; dx++)
            {
                for (int dy = 0; dy < height; dy++)
                {
                    var block = world.BlockAt(layer, x + dx, y + dy);
                    if (block.Block == PixelBlock.Empty && !copyEmpty) continue;
                    blocks.Add(new PlacedBlock(dx, dy, layer, block));
                }
            }
        }

        List<ITextLabel> labels = new List<ITextLabel>();
        var wholeWorld = (world.Width == width && world.Height == height);
        foreach (var placedTextLabel in world.GetLabels())
        {
            if (wholeWorld || (placedTextLabel.Label.Position.X / 16 >= x && placedTextLabel.Label.Position.X / 16 <= x + width))
            {
                if (wholeWorld || (placedTextLabel.Label.Position.Y / 16 >= y && placedTextLabel.Label.Position.Y / 16 <= y + height))
                {
                    var dx = placedTextLabel.Label.Position.X - x * 16;
                    var dy = placedTextLabel.Label.Position.Y - y * 16;

                    var label = new TextLabel(placedTextLabel.Label);
                    label.Position = new Point(dx, dy);
                    
                    labels.Add(label);
                }
            }
        }
        
        List<IZone> zones = new List<IZone>();
        foreach (var placedZone in world.GetZones())
        {
            var zone = new Zone(placedZone.Zone);

            if (!wholeWorld)
            {
                // Crop the zone's membership grid to the extracted region.
                var cropped = new bool[width, height];
                for (var dx = 0; dx < width; dx++)
                {
                    for (var dy = 0; dy < height; dy++)
                    {
                        cropped[dx, dy] = zone.IsBlockInZone(x + dx, y + dy);
                    }
                }

                zone.Width = width;
                zone.Height = height;
                zone.Membership = cropped;
            }

            zones.Add(zone);
        }

        return new Structure(width, height, new Dictionary<string, string>(), copyEmpty, blocks, labels, zones);
    }
    
    /// <summary>
    /// Get a structure from the world between two points.
    /// </summary>
    /// <param name="world"></param>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="copyEmpty"></param>
    /// <returns></returns>
    public static Structure GetStructure(this PixelWorld world, Point p1, Point p2, bool copyEmpty = false)
    {
        var topleft = new Point(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y));
        
        // Calculate width and height
        var width = Math.Abs(p1.X - p2.X);
        var height = Math.Abs(p1.Y - p2.Y);

        return GetStructure(world, topleft.X, topleft.Y, width + 1, height + 1, copyEmpty);
    }

    public static WorldStructure GetWorldStructure(this PixelWorld world, bool copyEmpty = false)
    {
        var structure = GetStructure(world, 0, 0, world.Width, world.Height, copyEmpty);
        var worldStructure = new WorldStructure(structure);
        worldStructure.WorldSettings = world.WorldSettings;
        
        return worldStructure;
    }

    /// <summary>
    /// A zone that needs to be created in the target world: the create request itself, plus the
    /// fully-translated target zone (name/settings/membership) needed to recognize the server's
    /// echoed creation and build its membership's area-edit requests afterward.
    /// </summary>
    public class ZoneCreation
    {
        public required IMessage UpsertPacket { get; init; }
        public required IZone TargetZone { get; init; }
    }

    public class WorldDifference
    {
        public required List<IPlacedBlock> Blocks { get; init; }
        public required List<ITextLabel> Labels { get; init; }

        /// <summary>
        /// Update-settings requests for zones that already exist in the target world (only sent
        /// when a setting actually differs).
        /// </summary>
        public required List<IMessage> ZoneUpserts { get; init; }

        /// <summary>
        /// Zones that don't exist in the target world yet and need to be created. See
        /// <see cref="ZoneCreation"/> — creating one and then populating its membership requires
        /// waiting for the server to echo back its assigned id (<see cref="PasteDifference"/>
        /// does this; <see cref="AsPackets"/> cannot, since it's synchronous, so it only sends the
        /// create request and leaves the new zone's membership empty).
        /// </summary>
        public required List<ZoneCreation> ZoneCreations { get; init; }

        /// <summary>
        /// Zone membership add/remove requests for zones that already exist in the target world
        /// (only the cells that changed).
        /// </summary>
        public required List<IMessage> ZoneAreaEdits { get; init; }

        public IEnumerable<IMessage> AsPackets()
        {
            return Blocks.ToChunkedPackets()
                .Concat(Labels.Select(l => l.ToUpsertPacket()))
                .Concat(ZoneUpserts)
                .Concat(ZoneCreations.Select(c => c.UpsertPacket))
                .Concat(ZoneAreaEdits);
        }
    }

    /// <summary>
    /// Get the difference between the world and structure at a specified place in the world.
    /// The returned list will be translated towards the origin point (unless disabled using the parameters) by copying the original blocks.
    /// If not translated the original references will be kept.
    /// </summary>
    /// <param name="world">The from which to check if blocks are already placed</param>
    /// <param name="structure">The structure to be placed.</param>
    /// <param name="x">The X location (Top Left)</param>
    /// <param name="y">The Y location (Top Left)</param>
    /// <param name="translate">If the output should be translated to the differences in the world.</param>
    /// <returns></returns>
    /// <exception cref="PixelGameException"></exception>
    public static WorldDifference GetDifference(this PixelWorld world, Structure structure, int x = 0, int y = 0, bool translate = true)
    {
        List<IPlacedBlock> difference = new();
        if (structure.Width + x > world.Width || structure.Height + y > world.Height)
            throw new PixelGameException(
                $"Attempted to a get the difference between a structure and world, but the structure goes outside the world! Structure size: {structure.Width}x{structure.Height} World size: {world.Width}x{world.Height}");
        
        foreach (var block in structure.BlocksWithEmpty)
        {
            var worldBlock = world.BlockAt(block.Layer, block.X + x, block.Y + y);
            if (block.Block.Equals(worldBlock)) continue;

            difference.Add(block);
        }
        
        // Get label difference.
        var labelDifference = structure.Labels.Where(structureLabel =>
        {
            // Modify the structure to get the correct comparison.
            var translatedLabel = new TextLabel(structureLabel);
            translatedLabel.Position = new Point(structureLabel.Position.X + (x * 16), structureLabel.Position.Y + (y * 16)); 
            
            // Rather inefficient, go through all labels and check if there is a matching one.
            foreach (var worldLabel in world.GetLabels())
            {
                if (worldLabel.Label.Equals(translatedLabel))
                {
                    return false;
                }
            }

            return true;
        }).ToList();

        // Zone difference. The server only lets a client change zone membership via rectangle
        // add/remove requests (never a raw membership blob), and a settings-upsert always needs
        // either no id (create) or an existing zone's id (update) — so, unlike blocks/labels,
        // zone packets are always built in world space here regardless of `translate`: matching a
        // structure zone to an existing world zone (by name) is what decides whether we can even
        // target an id for area edits.
        var worldZones = world.GetZones();
        var zoneUpserts = new List<IMessage>();
        var zoneCreations = new List<ZoneCreation>();
        var zoneAreaEdits = new List<IMessage>();

        foreach (var structureZone in structure.Zones)
        {
            var translatedZone = TranslateZoneToWorld(structureZone, world.Width, world.Height, x, y);
            var existingZone = worldZones.FirstOrDefault(z => z.Zone.Name == translatedZone.Name);

            if (existingZone == null)
            {
                // No matching zone in the target world: create it (empty id — the server assigns
                // its own; a non-empty but unrecognized id is treated as an edit of a
                // non-existent zone and silently does nothing). See ZoneCreation/PasteDifference
                // for how its membership gets populated once the server echoes back its new id.
                zoneCreations.Add(new ZoneCreation
                {
                    UpsertPacket = translatedZone.ToUpsertPacket(),
                    TargetZone = translatedZone
                });
                continue;
            }

            if (!translatedZone.SettingsEqual(existingZone.Zone))
            {
                zoneUpserts.Add(translatedZone.ToUpsertPacket(existingZone.Id));
            }

            var (add, remove) = ZoneMembershipRects.Diff(existingZone.Zone.Membership, translatedZone.Membership);
            zoneAreaEdits.AddRange(add.Select(rect => BuildAreaEditPacket(existingZone.Id, rect, true)));
            zoneAreaEdits.AddRange(remove.Select(rect => BuildAreaEditPacket(existingZone.Id, rect, false)));
        }

        // If not translated, just return the stucture differences.
        if (!translate)
        {
            return new WorldDifference()
            {
                Blocks = difference,
                Labels = labelDifference,
                ZoneUpserts = zoneUpserts,
                ZoneCreations = zoneCreations,
                ZoneAreaEdits = zoneAreaEdits
            };
        }

        // Deep copy the blocks and translate them.
        var translatedBlocks = difference.Select(pb => new PlacedBlock(pb.X + x, pb.Y + y, pb.Layer, (IPixelBlock) pb.Block.Clone())).ToList();
        var translatedLabels = labelDifference.Select(label =>
        {
            var translatedLabel = new TextLabel(label);
            translatedLabel.Position = new Point(label.Position.X + (x * 16), label.Position.Y + (y * 16));

            return translatedLabel;
        });

        return new WorldDifference()
        {
            Blocks = translatedBlocks.Cast<IPlacedBlock>().ToList(),
            Labels = translatedLabels.Cast<ITextLabel>().ToList(),
            ZoneUpserts = zoneUpserts,
            ZoneCreations = zoneCreations,
            ZoneAreaEdits = zoneAreaEdits
        };
    }

    private static IMessage BuildAreaEditPacket(string zoneId, Rectangle rect, bool add)
    {
        return new WorldZoneAreaEditRequestPacket
        {
            ZoneId = zoneId,
            X = rect.X,
            Y = rect.Y,
            Width = rect.Width,
            Height = rect.Height,
            Add = add,
        };
    }

    /// <summary>
    /// Expands a zone's membership grid into a world-sized grid positioned at (x, y).
    /// Cells outside the zone's own grid are left unset (not a member).
    /// </summary>
    private static Zone TranslateZoneToWorld(IZone zone, int worldWidth, int worldHeight, int x, int y)
    {
        var translated = new Zone(zone)
        {
            Width = worldWidth,
            Height = worldHeight,
            Membership = new bool[worldWidth, worldHeight]
        };

        for (var dx = 0; dx < zone.Width; dx++)
        {
            for (var dy = 0; dy < zone.Height; dy++)
            {
                var wx = dx + x;
                var wy = dy + y;
                if (wx < 0 || wy < 0 || wx >= worldWidth || wy >= worldHeight) continue;

                translated.Membership[wx, wy] = zone.Membership[dx, dy];
            }
        }

        return translated;
    }

    /// <summary>
    /// Sends a <see cref="WorldDifference"/> to the server. Blocks, labels, zone-settings updates
    /// and zone-creation requests are sent first. Zone area edits (membership add/remove) are
    /// sent afterward, delayed by <paramref name="areaEditDelayMs"/>, since an area edit targets a
    /// zone id that may only just have been created or had its settings changed moments before.
    /// For each <see cref="ZoneCreation"/>, this method starts waiting for the server to echo back
    /// the new zone's assigned id (via <see cref="PixelWorld.WaitForZoneUpsert"/>) *before* sending
    /// its create request, so the echo can't be missed; if it doesn't arrive within
    /// <paramref name="zoneCreationTimeoutMs"/>, that zone's membership is simply left empty.
    /// </summary>
    public static async Task PasteDifference(
        this PixelWorld world,
        PixelPilotClient client,
        WorldDifference difference,
        int zoneCreationTimeoutMs = 500)
    {
        // Subscribe before sending, so a fast echo can't arrive before we start listening.
        var pendingCreations = difference.ZoneCreations.Select(creation => (
            creation.TargetZone,
            Wait: world.WaitForZoneUpsert(z => z.SettingsEqual(creation.TargetZone), TimeSpan.FromMilliseconds(zoneCreationTimeoutMs))
        )).ToList();

        var packets = difference.Blocks.ToChunkedPackets();
        packets.AddRange(difference.Labels.Select(l => l.ToUpsertPacket()));
        packets.AddRange(difference.ZoneUpserts);
        packets.AddRange(difference.ZoneCreations.Select(c => c.UpsertPacket));

        if (packets.Count > 0)
        {
            client.SendRange(packets);
        }

        var newZoneAreaEdits = new List<IMessage>();
        foreach (var (targetZone, wait) in pendingCreations)
        {
            try
            {
                var createdZone = await wait;
                var rects = ZoneMembershipRects.Decompose(targetZone.Membership);
                newZoneAreaEdits.AddRange(rects.Select(rect => BuildAreaEditPacket(createdZone.Id, rect, true)));
            }
            catch (PixelGameException)
            {
                // Timed out waiting for the server to echo this zone's creation; its membership
                // is left empty rather than risk targeting the wrong (or no) zone id.
            }
        }

        var areaEdits = difference.ZoneAreaEdits.Concat(newZoneAreaEdits).ToList();
        if (areaEdits.Count == 0) return;
        
        client.SendRange(areaEdits);
    }

    public static async Task PasteSafe(this PixelWorld world, Structure structure, PixelPilotClient client, Point pasteLocation, int maxAttempts = 5)
    {
        int attempts = 0;
        while (attempts < maxAttempts)
        {
            var difference = world.GetDifference(structure, pasteLocation.X, pasteLocation.Y, true);
            
            // Blocks
            var diffPackets = difference.Blocks.ToChunkedPackets();
            
            // Labels
            diffPackets.AddRange(difference.Labels.Select(l => l.ToUpsertPacket()));

            // Zones
            diffPackets.AddRange(difference.ZoneUpserts);
            diffPackets.AddRange(difference.ZoneCreations.Select(c => c.UpsertPacket));
            diffPackets.AddRange(difference.ZoneAreaEdits);

            // If no diff, return.
            if (diffPackets.Count == 0) return;
            
            // Send packets. Assumes max ping 250.
            client.SendRange(diffPackets);
            await Task.Delay(250 + diffPackets.Count * 5);
            
            attempts++;
        }
        
        if (attempts >= maxAttempts) throw new Exception("Too many attempts to paste the structure!");
    }
}