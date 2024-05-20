# Pixel Pilot
[![NuGet Version](https://img.shields.io/nuget/vpre/PixelPilot.Core?style=flat-square&logo=nuget&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FPixelPilot.Core%2F%20)](https://www.nuget.org/packages/PixelPilot.Core/) 
![Build Project](https://img.shields.io/github/actions/workflow/status/MartenM/PixelPilot/dotnet.yml?branch=main&style=flat-square&logo=githubactions&logoColor=white&label=Build)
![Deploy documentation](https://img.shields.io/github/actions/workflow/status/MartenM/PixelPilot/docs.yml?branch=main&style=flat-square&logo=githubpages&label=Deploy%20Docs&link=https%3A%2F%2Fmartenm.github.io%2FPixelPilotDocs%2F)



A C# library for interacting with the game [PixelWalker](https://pixelwalker.net)

![Example bot](https://i.imgur.com/47bDpAc.gif)

## 📄 Documentation

- [https://martenm.github.io/PixelPilotDocs](https://martenm.github.io/PixelPilotDocs/guides/introduction.html)

## ✨ Features
* Strongly typed packets.
* PixelPilot.Core has minor abstracts
* Lightweight
* Useful helper classes (optional)



### 🛠 Projects:
* **PixelPilot.Core**: The core of the project. Bare minimum client to interact with the game.
* **PixelPilot.Tests**: All test related to the project.
* **PixelPilot.DebugTools**: Useful CLI programs to help development of PixelPilot.


## Example
```csharp
// Load the configuration. Don't store your account token in the code :)
var config = new ConfigurationBuilder()
    .AddJsonFile("config.json")
    .AddEnvironmentVariables()
    .Build().Get<BasicConfig>();

if (config == null)
{
    Console.WriteLine("The configuration file could not be loaded.");
    return;
}

// Create a client.
var client = new PixelPilotClient(config.AccountToken, false);

// Executed when the client receives a packet!
client.OnPacketReceived += (_, packet) =>
{
    switch (packet)
    {
        // Use strongly typed packets!
        case PlayerChatPacket { Message: ".stop" }:
            client.Disconnect();
            Environment.Exit(0);
            return;
    }
};

// Executed once the client receives INIT
// Make a platform and do some silly loops.
client.OnClientConnected += (_) =>
{
    client.Send(new PlayerChatOutPacket("Hello world using the PixelPilot API."));
};

// Connect to a room.
await client.Connect("r082b210d67df52");

// Don't terminate.
Thread.Sleep(-1);
```


Enjoy the library? Leave a ⭐

