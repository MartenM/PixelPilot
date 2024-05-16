---
uid: Guides.GettingStarted.ExtraTopics
title: Logging
---
# Extra topics
This article covers some of the mics features of `PixelPilot.Core`.

## Configuration
It's highly recommended that you don't store your token, username and or password in your code.
Rather opt for using a configuration file or .ENV variables instead.

Start by creating a config.json in your project. Ignore the logging part for now. It will be used in a later section of this guide.

```json
{
  "AccountToken": "Secret key value if you want to use the token login",
  "LoginEmail": "Email",
  "LoginPassword": "Password",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "PixelPilot.API": "Information",
      "PixelPilot.Client": "Information",
      "PixelPilot.World": "Information",
      "PixelPilot.PacketConverter": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  }
}
```

Create class the values can be mapped to:
```csharp
public class BasicConfig
{
    public string AccountToken { get; set; } = null!;
    public string AccountEmail { get; set; } = null!;
    public string AccountPassword { get; set; } = null!;
}
```

Initialize the configuration in your bot:
```csharp
var config = configuration.Get<BasicConfig>();
if (config == null)
{
    Console.WriteLine("The configuration file could not be loaded.");
    return;
}
```

You can now use `config.Token` to retrieve your token!

## Logging
PixelPilot uses the default logger provided with C#. In order to configure it you can use the following code snippet.
Note that this example uses the `config.json` from the previous step.

```csharp
LogManager.Configure(configuration.GetSection("Logging"));
var config = configuration.Get<BasicConfig>();
if (config == null)
{
    Console.WriteLine("The configuration file could not be loaded.");
    return;
}
```