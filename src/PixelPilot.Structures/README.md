# PixelPilot.Structures
Structures are used to load and save blocks. This is an optional package.

## 📄 Documentation
- [https://martenm.github.io/PixelPilotDocs](https://martenm.github.io/PixelPilotDocs/guides/introduction.html)

### JSON Format
This is an example. This example has been shortened to fit here so it might not actually be a valid and working save file.
```json
{
  // Version of this save
  "Version": 1,
  // Width & Height of this structure
  "Width": 1,
  "Height": 1,
  // Meta tags to be defined by the user
  "Meta": {
    "key": "value"
  },
  // If this structure has saved empty blocks
  "ContainsEmpty": false,
  "Blocks": {
    // Mapping of blocks. ID's in blockdata are replaced by temporary IDs.
    // BricksGrass: 0, Coin: 1
    "Mapping": [
      "BricksGrass",
      "Coin"
    ],
    // Blockdata as found in the world buffer in INIT.
    "BlockData": [
      "AQAAAAsAAAABAAAAAAAAAA==",
      "AgAAAAgAAAABAAAAAQAAAA=="
    ]
  }
}
```


## Example bot
An example bot can be found in [here](https://github.com/MartenM/PixelPilot/tree/main/examples/Example.StructuresBot).