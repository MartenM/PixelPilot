---
uid: Guides.ExtraPackages.PixelPilot.Structures
title: PixelPilot.ChatCommands
---
# Introduction
PixelPilot provides an additional package called `PixelPilot.ChatCommands`. This package allows users to execute commands in chat.

### Features:
* Supports permission strings
* Automatically generated `help` command.
* Format the help if you want.
* String based permissions. `root.command.subcommand.`
* Nested commands


## Getting started
Install `PixelPilot.ChatCommands` by using NuGet into your current project. Ensure that the `PixelPilot.Core` version matches the `PixelPilot.ChatCommands` version.

## Creating basic commands
To create a basic command, extend the `ChatCommand` class. Implement your own logic that should run on command.
```csharp
public class TestCommand : ChatCommand
{
    public TestCommand() : base("test", "A test command", null)
    {
        
    }

    public override Task ExecuteCommand(ICommandSender sender, string fullCommand, string[] args)
    {
        sender.SendMessage("This is a test command!");
        return Task.CompletedTask;
    }
}
```

Now create a command manager and hook it to the client. Register the command and add the optional help command.
```csharp
var commandManager = new CustomCommandManager(client, playerManager);
commandManager.AddHelpCommand();
commandManager.AddCommand(new TestCommand());
```

That's it! Your command can now be executed in the game.

## Nested commands
To nest commands use extend the `RootCommand`. Add your new commands in the constructor of this command.
Note that these can also be RootCommands.
```csharp
public class BotActionsRoot : RootCommand
{
    public BotActionsRoot(PixelPilotClient client) : base("bot", "Bot commands", "bot")
    {
        // Add the commands.
        AddCommand(new DisconnectCommand(client));;
        AddCommand(new BroadcastCommand(client));;
    }
}
```

Note that the base permission for this command is `bot`. The commands `Disconnect` and `Broadcast` extend this permission node by using the `+` sign.
This means that the full permission will be `bot.disconnect`.
```csharp
public class SubCommand : ChatCommand
{
    private PixelPilotClient _client;
    
    public DisconnectCommand(PixelPilotClient client) : base("disconnect", "Disconnect the bot", "+disconnect")
    {
        _client = client;
    }

    public override Task ExecuteCommand(ICommandSender sender, string fullCommand, string[] args)
    {
        // Logic
        return Task.CompletedTask;
    }
}
```

## Handling permissions
By default the bot handles permissions by allowing all. You can change this by implementing your own command manager. See the following example below.
Based on the players rank we grab a list of permission nodes allowed. If the list contains the permission node, the command is allowed.
```csharp
public class CustomCommandManager : PixelChatCommandManager<Player>
{
    private PixelPilotClient _client;
    
    public CustomCommandManager(PixelPilotClient client, PixelPlayerManager<Player> pixelPlayerManager) : base(client, pixelPlayerManager)
    {
        _client = client;
    }
    
    protected override ICommandSender CreateSender(Player player)
    {
        // Create a custom sender that executes the permission check.
        return new CustomSender(player, _client);
    }
}

class CustomSender : CommandSender
{
    public CustomSender(IPixelPlayer player, PixelPilotClient client) : base(player, client)
    {
        
    }

    public override bool HasPermission(string? permission)
    {
        // Allow by default if no permission is set.
        if (permission == null) return true;
        
        // Normally fetch the players rank here from the IPixelPlayer.
        var playerRank = Rank.Default;
        var permissions = playerRank.GetPermissions();
        return permissions.Contains(permission);
    }
}
```