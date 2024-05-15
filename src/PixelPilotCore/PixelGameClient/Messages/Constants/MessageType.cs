namespace PixelPilot.PixelGameClient.Messages.Constants;

public enum MessageType : byte
{
    /**
     * Generic message types. Used for connection states etc.
     */
    Ping = 0x3F,
    
    /**
     * Messages related to the world that yield information about the world.
     */
    World = 0x6B
}