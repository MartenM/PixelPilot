﻿using PixelPilot.ChatCommands;
using PixelPilot.ChatCommands.Commands;
using PixelPilot.Client;

namespace Example.CommandBot.Commands.SubCommand;

public class BroadcastCommand : ChatCommand
{
    private PixelPilotClient _client;
    
    public BroadcastCommand(PixelPilotClient client) : base("broadcast", "Broadcast a message", "+broadcast")
    {
        _client = client;
    }

    public override Task ExecuteCommand(ICommandSender sender, string fullCommand, string[] args)
    {
        _client.SendChat($"{string.Join(' ', args)}");
        return Task.CompletedTask;
    }
}