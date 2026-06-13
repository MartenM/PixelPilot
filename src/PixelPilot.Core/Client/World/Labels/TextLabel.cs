using System.Drawing;
using Google.Protobuf;
using PixelPilot.Client.Extensions;
using PixelPilot.Client.Messages.Packets.Extensions;
using PixelWalker.Networking.Protobuf.WorldPackets;

namespace PixelPilot.Client.World.Labels;

public class TextLabel : ITextLabel
{
    public Point Position { get; set; }

    public string Text { get; set; } = "";

    public Color Color { get; set; }

    public float MaxWidth { get; set; }

    public bool Shadow { get; set; }

    public LabelTextAlignment TextAlignment { get; set; }

    public int FontSize { get; set; }

    public float CharacterSpacing { get; set; }

    public float LineSpacing { get; set; }

    public int RenderLayer { get; set; }

    public Color ShadowColor { get; set; }

    public int ShadowOffsetX { get; set; }

    public int ShadowOffsetY { get; set; }
    
    public bool OutlineEnabled { get; set; }
    public int OutlineWidth { get; set; }
    public Color OutlineColor { get; set; }
    

    public TextLabel() {}

    public TextLabel(ITextLabel label)
    {
        CharacterSpacing = label.CharacterSpacing;
        Color = label.Color;
        FontSize = label.FontSize;
        LineSpacing = label.LineSpacing;
        MaxWidth = label.MaxWidth;
        RenderLayer = label.RenderLayer;
        Shadow = label.Shadow;
        ShadowColor = label.ShadowColor;
        ShadowOffsetX = label.ShadowOffsetX;
        ShadowOffsetY = label.ShadowOffsetY;
        Position = label.Position;
        TextAlignment = label.TextAlignment;
        Text = label.Text;

        OutlineEnabled = label.OutlineEnabled;
        OutlineWidth = label.OutlineWidth;
        OutlineColor = label.OutlineColor;
    }

    public static TextLabel FromProtoTextLabel(ProtoTextLabel protoTextLabel)
    {
        var thisLabel = new TextLabel();
        thisLabel.UpdateWithProtoTextLabel(protoTextLabel);
        return thisLabel;
    }

    protected bool Equals(TextLabel other)
    {
        return Position.Equals(other.Position) && 
               Text == other.Text && 
               Color.Equals(other.Color) && 
               MaxWidth.Equals(other.MaxWidth) && 
               Shadow == other.Shadow && 
               TextAlignment == other.TextAlignment && 
               FontSize == other.FontSize && 
               CharacterSpacing.Equals(other.CharacterSpacing) && 
               LineSpacing.Equals(other.LineSpacing) && 
               RenderLayer == other.RenderLayer && 
               ShadowColor.Equals(other.ShadowColor) && 
               ShadowOffsetX == other.ShadowOffsetX && 
               ShadowOffsetY == other.ShadowOffsetY && 
               OutlineEnabled == other.OutlineEnabled &&
               OutlineColor ==  other.OutlineColor &&
               OutlineWidth == other.OutlineWidth;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((TextLabel)obj);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(Position);
        hashCode.Add(Text);
        hashCode.Add(Color);
        hashCode.Add(MaxWidth);
        hashCode.Add(Shadow);
        hashCode.Add((int)TextAlignment);
        hashCode.Add(FontSize);
        hashCode.Add(CharacterSpacing);
        hashCode.Add(LineSpacing);
        hashCode.Add(RenderLayer);
        hashCode.Add(ShadowColor);
        hashCode.Add(ShadowOffsetX);
        hashCode.Add(ShadowOffsetY);
        hashCode.Add(OutlineColor);
        hashCode.Add(OutlineWidth);
        hashCode.Add(OutlineEnabled);
        return hashCode.ToHashCode();
    }

    public ProtoTextLabel AsProtoTextLabel()
    {
        return new ProtoTextLabel
        {
            CharacterSpacing = CharacterSpacing,
            Color = Color.ToUint(),
            FontSize = FontSize,
            LineSpacing = LineSpacing,
            MaxWidth = MaxWidth,
            RenderLayer = RenderLayer,
            Shadow = Shadow,
            ShadowColor = ShadowColor.ToUint(),
            ShadowOffsetX = ShadowOffsetX,
            ShadowOffsetY = ShadowOffsetY,
            Position = Position.ToPoint(),
            TextAlignment = TextAlignment.ToProtoTextLabelAlignment(),
            Text = Text,
            Outline = OutlineEnabled,
            OutlineWidth = OutlineWidth,
            OutlineColor = OutlineColor.ToUint()
        };
    }

    public IMessage ToUpsertPacket(string? flowId = null)
    {
        return new WorldLabelUpsertRequestPacket()
        {
            Label = AsProtoTextLabel(),
        };
    }

    public void UpdateWithProtoTextLabel(ProtoTextLabel label)
    {
        CharacterSpacing = label.CharacterSpacing;
        Color = label.Color.ToColor();
        FontSize = label.FontSize;
        LineSpacing = label.LineSpacing;
        MaxWidth = label.MaxWidth;
        RenderLayer = label.RenderLayer;
        Shadow = label.Shadow;
        ShadowColor = label.ShadowColor.ToColor();
        ShadowOffsetX = label.ShadowOffsetX;
        ShadowOffsetY = label.ShadowOffsetY;
        Position = label.Position.ToPoint();
        TextAlignment = label.TextAlignment.ToLabelTextAlignment();
        Text = label.Text;

        OutlineEnabled = label.Outline;
        OutlineWidth = label.OutlineWidth;
        OutlineColor = label.OutlineColor.ToColor();
    }
    
    
}