---
uid: Guides.GettingStarted.PlacingBlocks
title: Manipulating the world
---
# Manipulating the world
In the previous examples we have seen how to use basic packets. In this guide we will start manipulating the world. That's all what this game is about in the end!

## The World class
In order to make World manipulation a bit easier `PixelPilot.Core` exposes a `PixelWorld` class.
To parse the block packets can be quite complicated, so if you don't want to do this yourself, I highly recommend you use this class.

To get started with the `PixelWorld` class, create an instance of it and ensure it receives packet updates from the client.

```csharp
// Create a PixelWorld class and attach the client to it.
// Allow it to listen to client updates. Not required!
var world = new PixelWorld();
client.OnPacketReceived += world.HandlePacket;
```

We now have a `world` object that will keep track of blocks in the world, and dispatch events on block changes.
The world has several layers. Each layer is used for a specific type of block. Currently there are two layers. The `WorldLayer` enum can be used for simplicity.

| Layer | Description  | Enum                  |
|-------|--------------|-----------------------|
| 0     | Background   | WorldLayer.Background |
| 1     | Foreground   | WorldLayer.Foreground |

In order to get a block at a specific coordinate and layer you can use the following snippet.
We then check if it's a coin. Note that the `block.Block` gives us an enum. This enum can be cast to an INT if required.
```csharp
var worldBlock = world.BlockAt(layer, x, y);
Console.WriteLine($"Is this a coin? ({worldBlock.Block == PixelBlock.Coin})");
Console.WriteLine($"It is a: {worldBlock.Block} with ID {block.BlockId}");
```

### Blocks with additional data.
Some blocks contain additional data. Think about portals, gates, signs, etc.
To access this data, you can simply cast the IPixelBlock to it's desired type. In this example we will check for portal block.
```csharp
var worldBlock = world.BlockAt(layer, x, y);
if (worldBlock is PortalBlock portalBlock)
{
    Console.WriteLine($"It is a portal with target {portalBlock.TargetId}");
}
else
{
    Console.WriteLine("The block is not a portal.");
}
```

> [!TIP]
> The 'is' keyword is used to cast the object. For more information about casting check [here](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/types/casting-and-type-conversions)

### Sending blocks
In order to send your own blocks you can simply create an instance of the class.
A block placement also needs a location, so we wrap the block with the `PlacedBlock` class.
```csharp
BasicBlock block = new BasicBlock((int) PixelBlock.Crown);
PlacedBlock placedBlock = new PlacedBlock(x, y, WorldLayer.Foreground, block);
client.Send(placedBlock.AsPacketOut());
```
That's it, you have send a block!

### Example: Blocking the crown!
Lets say we want to disable people from placing a crown block. In order to do this, we need to listen to any blocks being placed in our world.
The following code snippet can be used to achieve the actions we want.

```csharp
world.OnBlockPlaced += (_, playerId, oldBlock, newBlock) =>
{
    // Ignore our own bot
    if (client.BotId == playerId) return;

    if (newBlock.Block.Block != PixelBlock.Crown) return;
    client.Send(oldBlock.AsPacketOut());
};
```