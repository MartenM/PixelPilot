---
uid: Guides.GettingStarted.PlayerManagement
title: Player Management
---
# Player Management
The game only sends updates about players only once. In order to make keep track of players in your world you can use the `PlayerManager`.
This manager keeps tracks of players in your world and easily allows you to fetch their latest information.

The following code snippet initializes the manager and ensures it receives the packets from the client.
```csharp
// Player manager allows you to easily keep track of player stats.
// For advanced users, it can be extended to include relevant information for you.
var playerManager = new PlayerManager();
client.OnPacketReceived += playerManager.HandlePacket;
```

You now have access to the players stats at all time. This can be used in for example a `.ping` command that sends back the players name.
```csharp
client.OnPacketReceived += (_, packet) =>
{
    // Make use of strongly typed packets!
    switch (packet)
    {
        case PlayerChatPacket { Message: ".ping" } chat:
        {
            var player = playerManager.GetPlayer(chat.PlayerId);
            if (player == null) return;

            client.Send(new PlayerChatOutPacket($"Pong! ({player.Username}, {player.X}, {player.Y})"));
            break;
        }
    }
};
```

## Advanced usage: Making your own player class
When making a minigame you might need to store more information about the player. The API allows you to define your own player class that can still be used by the manager.
The following steps should be taken:
1. Create your own Player class that implements `IPixelPlayer`.
2. Create your own PlayerManager which extends `PixelPlayerManager`.
3. Start using your own implementation of `IPixelPlayer` and `PixelPlayerManger`!

The default classes `Player` and `PlayerManager` used in this guide are created in the same way.