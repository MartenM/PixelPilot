namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerTouchPlayerPacket : IPixelGamePlayerPacket
{
    public PlayerTouchPlayerPacket(int id, int touchedPlayer, byte isTouching)
    {
        PlayerId = id;
        TouchedPlayer = touchedPlayer;
        IsToucing = Convert.ToBoolean(isTouching);
    }

    public int PlayerId { get; }
    public int TouchedPlayer { get; }
    public bool IsToucing { get; }
}