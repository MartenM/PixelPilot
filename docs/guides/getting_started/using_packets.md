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

All incoming packets implement the interface `IPixelGamePacket`. Packets that are related to a player implement an addtional interface called `IPixelGamePlayerPacket`.
Packets that you can send implement the interface 'IPixelGamePacketOut'. We will get to sending packets in a bit. 
A list of all incoming and outgoing packets can be found here:

| Packet Type | Documentation                                       |
|-------------|-----------------------------------------------------|
| Incoming    | <xref:PixelPilot.PixelGameClient.Messages.Received> |
| Outgoing    | <xref:PixelPilot.PixelGameClient.Messages.Send>     |

## Handling specific packets (Giving god on join)
Executing something on each packet received is not that useful. Luckily, with the use of some casting we can easily execute actions when we receive a specific packet.
Since the packets are strongly typed, we can use a switch statement to do some more useful things. In the following code snippet, we check for the join packet. If we get it we get the username from it.
After that we send the `PlayerChatOutPacket` with the username of the joined player.
```csharp
// Make use of strongly typed packets!
switch (packet)
{
    case PlayerJoinPacket join:
        client.Send(new PlayerChatOutPacket($"/givegod {join.Username}"));
        break;
}
```

All packets that can be send contain the word `Out`. This indicates that it's an outgoing packet. All outgoing packets can be found in the previously seen table. For blocks, there is a different method of constructing the packet which we will see in a later guide.
We will now extend this example with a simple `.stop` command for the bot. Note that everyone can stop the bot.

```csharp
// Make use of strongly typed packets!
switch (packet)
{
    case PlayerChatPacket { Message: ".stop" }:
        client.Disconnect();
        Environment.Exit(0);
        return;
    case PlayerJoinPacket join:
        client.Send(new PlayerChatOutPacket($"/givegod {join.Username}"));
        break;
}
```

That's it, you made a simple bot that responds to incoming packets and sends packets to the game!