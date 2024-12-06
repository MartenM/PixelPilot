---
uid: Guides.GettingStarted.UsingPackets
title: Using Packets
---
# Using packets
The game communicates with your bot using `Packets` (Block placed, Smiley changed, Player moved, etc). `PixelPilot.Core` provides some abstraction so you don't have to construct these yourself.
To start listening to incoming packets the game simply use the `OnPacketReceived` event handler of the client.
```csharp
// Executed when the client receives a packet!
client.OnPacketReceived += (_, packet) =>
{
    Console.WriteLine("I received a packet");    
}
```

All incoming packets implement the interface `IMessage`. A list of the packets can be found in the following documentation:

| Packet   | Documentation                                            |
|----------|----------------------------------------------------------|
| All      | <xref:PixelWalker.Networking.Protobuf.WorldPackets> |

## Handling specific packets (Giving god on join)
Executing something on each packet received is not that useful. Luckily, with the use of some casting we can easily execute actions when we receive a specific packet.
Since the packets are strongly typed, we can use a switch statement to do some more useful things. In the following code snippet, we check for the join packet. If we get it we get the username from it.
After that we send the `PlayerChatPacket` with the username of the joined player.
```csharp
// Make use of strongly typed packets!
switch (packet)
{
    case PlayerJoinedPacket joinData:
        client.Send(new PlayerChatPacket()
        {
            Message = $"/givegod {joinData.Properties.Username}"
        });
        break;
}
```

For blocks, there is a different method of constructing the packet which we will see in a later guide.
We will now extend this example with a simple `.stop` command for the bot. Note that everyone can stop the bot.

```csharp
// Make use of strongly typed packets!
switch (packet)
{
    case PlayerChatPacket { Message: ".stop" }:
        client.Disconnect();
        Environment.Exit(0);
        return;
    case PlayerJoinedPacket joinData:
        client.Send(new PlayerChatPacket()
        {
            Message = $"/givegod {joinData.Properties.Username}"
        });
        break;
}
```

That's it, you made a simple bot that responds to incoming packets and sends packets to the game!