namespace PixelPilot.Api;

/**
 * Thrown by the PixelApi.
 */
public class PixelApiException : Exception
{
    public PixelApiException(string msg) : base(msg)
    {
        
    }
}