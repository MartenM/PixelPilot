---
uid: Guides.GettingStarted.FirstBot
title: Start making a bot
---

> [!NOTE]
> This guide assumes you have basic knowledge about C# and NuGet. 
> If you don't, start learning it today!

# Installation
The main package `PixelPilot.Core` can be found on NuGet. Install the latest version to start using the API.

# Your first bot
`PixelPilot.Core` makes it easy to get a bot up and running. You can either provide the `PixelPilotClient` with a token, or by using your email and password directly.
The token can be found in the local storage of your browser.
```csharp
# Create a bot by using a token
var client = new PixelPilotClient("Your account token");

# Create a bot by using 
var client = new PixelPilotClient("bot@pixelpilot.com", "super secret password");
```

After creating the client you probably, you can let the bot connect to a world.
Each world has an unique world ID. When you join a world, the URL bar will display the following `https://pixelwalker.net/world/<WORLD_ID>`.
Copy the world ID and use it to connect to the world.
```csharp
// Connect to a room.
await client.Connect("r082b210d67df52");
```

Our bot has now joined the world but it since the end of the program has been reached it will terminate.
You can prevent this by adding the following code to the end of your program.
```csharp
// Don't terminate.
Thread.Sleep(-1);
```

That's it, you have now connected your first bot to the world!