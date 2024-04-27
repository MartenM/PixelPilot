namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerJoinPacket : IPixelGamePacket
{
    public PlayerJoinPacket(int id, string cuid, string username, int face, bool isAdmin, double x, double y, int unknownA, int unknownB, int unknownC, bool godmode, bool modmode, bool hasCrown, byte[] buffer)
    {
        Id = id;
        Cuid = cuid;
        Username = username;
        Face = face;
        IsAdmin = isAdmin;
        X = x;
        Y = y;
        UnknownA = unknownA;
        UnknownB = unknownB;
        UnknownC = unknownC;
        Godmode = godmode;
        Modmode = modmode;
        HasCrown = hasCrown;
        Buffer = buffer;
    }

    public int Id { get; }
    public string Cuid { get; }
    public string Username { get; }
    public int Face { get; }
    public bool IsAdmin { get; }
    public double X { get; }
    public double Y { get; }
    public int UnknownA { get; }
    public int UnknownB { get; }
    public int UnknownC { get; }
    public bool Godmode { get; }
    public bool Modmode { get; }
    public bool HasCrown { get; }
    public byte[] Buffer { get; }
}