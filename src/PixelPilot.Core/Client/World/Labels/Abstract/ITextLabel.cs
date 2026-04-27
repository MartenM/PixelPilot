using System.Drawing;
using Google.Protobuf;
using PixelWalker.Networking.Protobuf.WorldPackets;

namespace PixelPilot.Client.World.Labels;

public interface ITextLabel
{
    public Point Position { get; }
    
    public string Text { get; }

    public Color Color { get; }

    public float MaxWidth { get; }

    public bool Shadow { get; }

    public LabelTextAlignment TextAlignment { get; }

    public int FontSize { get; }

    public float CharacterSpacing { get; }

    public float LineSpacing { get; }

    public int RenderLayer { get; }

    public Color ShadowColor { get; }

    public int ShadowOffsetX { get; }

    public int ShadowOffsetY { get; }

    public ProtoTextLabel AsProtoTextLabel();

    public IMessage ToUpsertPacket(string? flowId = null);
}