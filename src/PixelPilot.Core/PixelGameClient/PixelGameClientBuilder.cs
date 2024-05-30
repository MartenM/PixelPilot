using PixelPilot.PixelHttpClient;

namespace PixelPilot.PixelGameClient;

public class PixelGameClientBuilder
{
    public string? Token { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }

    public bool AutomaticReconnect { get; set; } = false;

    public PixelGameClientBuilder SetToken(string token)
    {
        Token = token;
        return this;
    }
    
    public PixelGameClientBuilder SetEmail(string email)
    {
        Email = email;
        return this;
    }
    
    public PixelGameClientBuilder SetPassword(string password)
    {
        Password = password;
        return this;
    }

    public PixelGameClientBuilder SetAutomaticReconnect(bool value)
    {
        AutomaticReconnect = value;
        return this;
    }

    public PixelPilotClient Build()
    {
        PixelApiClient api;
        if (Token != null)
        {
            api = new PixelApiClient(Token);
        }
        else if (Email != null && Password != null)
        {
            api = new PixelApiClient(Email, Password);
        }
        else
        {
            throw new PixelGameException("To create a client a Token or (Email & Password) should be supplied.");
        }

        return new PixelPilotClient(api, AutomaticReconnect);
    }
}