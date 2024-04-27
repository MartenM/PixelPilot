namespace PixelPilot.PixelGameClient.Messages.Received;

public class InitPacket : IPixelGamePacket
{
    public int Id { get; set; }
    public string CId { get; set; }
    public string Username { get; set; }
    public int Face { get; set; }
    public bool IsAdmin { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public bool CanEdit { get; set; }
    public bool CanGod { get; set; }
    public string RoomTitle { get; set; }
    public int Players { get; set; }
    public string Owner { get; set; }
    public byte[] GlobalSwitchStates { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public byte[] Buffer { get; set; }
    
    public InitPacket(int id, string cId, string username, int face, bool isAdmin, double x, double y, bool canEdit, bool canGod, string roomTitle, int players, string owner, byte[] globalSwitchStates, int width, int height, byte[] buffer)
    {
        Id = id;
        CId = cId;
        Username = username;
        Face = face;
        X = x;
        Y = y;
        CanEdit = canEdit;
        CanGod = canGod;
        RoomTitle = roomTitle;
        Players = players;
        Owner = owner;
        GlobalSwitchStates = globalSwitchStates;
        Width = width;
        Height = height;
        Buffer = buffer;
        IsAdmin = isAdmin;
    }

    public static byte[] AsSendingBytes() => new byte[] { 0x6b, 0x00 };
}