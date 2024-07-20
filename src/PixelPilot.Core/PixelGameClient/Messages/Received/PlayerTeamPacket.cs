namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerTeamPacket : IPixelGamePlayerPacket
{
    public PlayerTeamPacket(int id, int team)
    {
        PlayerId = id;
        Team = team;
    }

    public int PlayerId { get; }
    public int Team { get; }
}