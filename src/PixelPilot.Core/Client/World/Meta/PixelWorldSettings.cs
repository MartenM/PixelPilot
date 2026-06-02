using System.Drawing;
using PixelPilot.Client.Extensions;
using PixelWalker.Networking.Protobuf.WorldPackets;

namespace PixelPilot.Client.World.Meta;

/// <summary>
/// Settings of a world that can be changed by the builder of a world.
/// </summary>
public class PixelWorldSettings
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int MaxPlayers { get; set; }
    public string Visibility { get; set; }
    
    public bool MinimapEnabled { get; set; }
    public bool MinimapScaled { get; set; }
    public int MinimapMaxWidth { get; set; }
    public int MinimapMaxHeight { get; set; }
    public int MinimapEdgeFadeWidth { get; set; }
    
    public bool HasBackgroundColor { get; set; }
    public Color BackgroundColor { get; set; }
    
    public bool HasVoidColor { get; set; }
    public Color VoidColor { get; set; }

    public PixelWorldSettings()
    {
        
    }

    public PixelWorldSettings(WorldMeta worldMeta)
    {
        Title = worldMeta.Title;
        Description = worldMeta.Description;
        MaxPlayers = worldMeta.MaxPlayers;
        Visibility = worldMeta.Visibility;
        
        
        MinimapEnabled = worldMeta.MinimapEnabled;
        MinimapScaled = worldMeta.MinimapScaled;
        MinimapMaxWidth = worldMeta.MinimapMaxWidth;
        MinimapMaxHeight = worldMeta.MinimapMaxHeight;
        MinimapEdgeFadeWidth = worldMeta.MinimapEdgeFadeWidth;
        HasBackgroundColor = worldMeta.HasBackgroundColor;
        BackgroundColor = worldMeta.BackgroundColor.ToColor();
        HasVoidColor = worldMeta.HasVoidColor;
        VoidColor = worldMeta.VoidColor.ToColor();
    }
    
    private void ApplyTo(WorldMeta worldMeta)
    {
        worldMeta.Title = Title;
        worldMeta.Description = Description;
        worldMeta.MaxPlayers = MaxPlayers;
        worldMeta.Visibility = Visibility;

        worldMeta.MinimapEnabled = MinimapEnabled;
        worldMeta.MinimapScaled = MinimapScaled;
        worldMeta.MinimapMaxWidth = MinimapMaxWidth;
        worldMeta.MinimapMaxHeight = MinimapMaxHeight;
        worldMeta.MinimapEdgeFadeWidth = MinimapEdgeFadeWidth;

        worldMeta.HasBackgroundColor = HasBackgroundColor;
        worldMeta.BackgroundColor = BackgroundColor.ToInt(); // or equivalent conversion

        worldMeta.HasVoidColor = HasVoidColor;
        worldMeta.VoidColor = VoidColor.ToInt(); // or equivalent conversion
    }

    public WorldMetaUpdatePacket AsUpdatePacket()
    {
        var packet = new WorldMetaUpdatePacket();
        packet.Meta = new WorldMeta();
        
        ApplyTo(packet.Meta);

        return packet;
    }
}