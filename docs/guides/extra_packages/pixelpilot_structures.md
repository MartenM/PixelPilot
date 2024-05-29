---
uid: Guides.ExtraPackages.PixelPilot.Structures
title: PixelPilot.Structures
---
# Introduction
PixelPilot provides an additional package called `PixelPilot.Structures`. This package allows users to save, load and share structures within their worlds (or complete worlds!)

## Getting started
Install `PixelPilot.Structures` by using NuGet into your current project. Ensure that the `PixelPilot.Core` version matches the `PixelPilot.Structures` version.

## Creating & Saving a structure
A structure can be grabbed from a world and serialized to a JSON document.
This JSON document has a special encoding for blocks which is not human readable. It does ensure that in future versions of the game you can still load old structures.
```csharp
// Create a structure, don't save empty blocks.
var structure = world.GetStructure(p1, p2, copyEmpty: false);

// Convert the structure to the JSON format & save it.
var json = PilotSaveSerializer.Serialize(structure);
File.WriteAllText("test-struct.json", json);
```

## Loading & Pasting a structure
```csharp
// Load the file and convert it to a structure.
string json = File.ReadAllText("test-struct.json");
var structure = PilotSaveSerializer.Deserialize(json);

// Various methods for getting the list of blocks.
List<IPlacedBlock> diff = world.GetDifference(structure, x, y);
List<IPlacedBlock> blocks = structure.Blocks;
List<IPlacedBlock> blocks = structure.BlocksWithEmpty;

// Helper methods for pasting the blocks.
structure.Blocks.PasteInOrder(client, new Point(x, y), 5);
structure.Blocks.PasteShuffeled(client, new Point(x, y), 5);
```