namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerUpdateRightsPacket : IPixelGamePacket
{
     public PlayerUpdateRightsPacket(bool editRights, bool godmode)
     {
          EditRights = editRights;
          Godmode = godmode;
     }

     public bool EditRights { get; }
     public bool Godmode { get; }
}