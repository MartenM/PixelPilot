﻿using PixelPilot.Api;
using PixelPilot.Client.Messages.Queue;

namespace PixelPilot.Client;

public class PixelGameClientBuilder
{
    public string? Token { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }

    public PixelApiClient? ApiClient;
    public bool AutomaticReconnect { get; set; } = false;

    public string? Prefix { get; set; }

    public Func<PixelPilotClient, IPixelPacketQueue?> ConfigurePacketQueue { get; set; } = client => new TokenBucketPacketOutQueue(client);

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

    /// <summary>
    /// Set the prefix that the bot will use outgoing chat messages.
    /// </summary>
    /// <param name="prefix"></param>
    /// <returns></returns>
    public PixelGameClientBuilder SetPrefix(string? prefix)
    {
        Prefix = prefix;
        return this;
    }

    /// <summary>
    /// Disable the internal packet queue that prevents messages from being
    /// caught by the PW rate limiter.
    /// </summary>
    /// <returns></returns>
    public PixelGameClientBuilder DisablePacketQueue()
    {
        ConfigurePacketQueue = _ => null;
        return this;
    }

    /// <summary>
    /// Use a custom PixelApiClient
    /// </summary>
    /// <param name="client">The custom client</param>
    /// <returns></returns>
    public PixelGameClientBuilder SetPixelApi(PixelApiClient? client)
    {
        ApiClient = client;
        return this;
    }

    public PixelPilotClient Build()
    {
        if (ApiClient == null)
        {
            // API client needs to be created
            if (Token != null)
            {
                ApiClient = new PixelApiClient(Token);
            }
            else if (Email != null && Password != null)
            {
                ApiClient = new PixelApiClient(Email, Password);
            }
            else
            {
                throw new PixelGameException("To create a client a Token or (Email & Password) should be supplied.");
            }
        }
        
        return new PixelPilotClient(ApiClient, AutomaticReconnect, Prefix, ConfigurePacketQueue);
    }
}