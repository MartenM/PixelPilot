namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerUpdateRightsPacket : IPixelGamePacket
{
     public PlayerUpdateRightsPacket(int id, bool editRights, bool godmode)
     {
          EditRights = editRights;
          Godmode = godmode;
          Id = id;
     }
     public int Id { get; }
     public bool EditRights { get; }
     public bool Godmode { get; }
}