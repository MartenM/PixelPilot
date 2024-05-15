namespace PixelPilot.PixelGameClient.Messages.Exceptions;

public abstract class PixelException : Exception
{
    protected PixelException(string msg) : base(msg)
    {
        
    }
}