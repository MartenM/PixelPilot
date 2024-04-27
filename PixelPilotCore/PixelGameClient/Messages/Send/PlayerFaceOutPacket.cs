using PixelPilot.PixelGameClient.Messages.Constants;

namespace PixelPilot.PixelGameClient.Messages.Send;

public class PlayerFaceOutPacket : ReflectivePixelOutPacket
{
    public int Face { get; set; }
    
    public PlayerFaceOutPacket(int face) : base(WorldMessageType.PlayerFace)
    {
        Face = face;
    }
}