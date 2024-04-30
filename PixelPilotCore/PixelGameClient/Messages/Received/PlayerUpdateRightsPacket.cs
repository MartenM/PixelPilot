namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerUpdateRightsPacket : IPixelGamePacket
{
     public PlayerUpdateRightsPacket(int id, bool editRights, bool godmode)
     {
          EditRights = editRights;
          Godmode = godmode;
          PlayerId = id;
     }
     public int PlayerId { get; }
     public bool EditRights { get; }
     public bool Godmode { get; }
}