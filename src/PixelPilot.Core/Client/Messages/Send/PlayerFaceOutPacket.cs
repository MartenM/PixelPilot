using PixelPilot.Client.Messages.Constants;

namespace PixelPilot.Client.Messages.Send;

public class PlayerFaceOutPacket : ReflectivePixelOutPacket
{
    public int Face { get; set; }
    
    public PlayerFaceOutPacket(int face) : base(WorldMessageType.PlayerFace)
    {
        Face = face;
    }
}