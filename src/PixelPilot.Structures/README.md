# PixelPilot.Structures
Structures are used to load and save blocks. This is an optional package.

## 📄 Documentation
- [https://martenm.github.io/PixelPilotDocs](https://martenm.github.io/PixelPilotDocs/guides/introduction.html)

## Features

- Build a `Structure`/`WorldStructure` from a live `PixelWorld`, including width/height, meta
  tags, blocks, labels, zones and world settings.
- Serialize/deserialize structures to and from JSON via `PilotSaveSerializer`, without needing to
  know which format a given save file is in.
- Compact on-disk format (Pilot2): blocks are pallet-mapped so repeated block types/fields aren't
  duplicated per position.
- Automatic migration of older save files to the current format when loading, both for the
  current (Pilot2) and legacy (PilotSimple) schemas.
- Backwards-compatible reading of legacy save files (`Version < 20`) alongside the current format.

### JSON Format
This is an example of the current (Pilot2) format. This example has been shortened to fit here so
it might not actually be a valid and working save file.
```json
{
  // Version of this save (>= 20 selects the Pilot2 format)
  "Version": 20,
  "Width": 1,
  "Height": 1,
  // Meta tags to be defined by the user (also carries WorldSettings, when set)
  "Meta": {
    "key": "value"
  },
  "BlocksVersion": 1,
  "Labels": [],
  "Zones": [],
  // Distinct blocks (incl. their fields), referenced by index from BlockReferences.
  "BlockPallet": [
    { "Name": "BricksGrass", "Fields": {} },
    { "Name": "Coin", "Fields": {} }
  ],
  // Each entry is [Layer, X, Y, PalletIndex].
  "BlockReferences": [
    [1, 0, 0, 0],
    [1, 1, 0, 1]
  ]
}
```


## Example bot
An example bot can be found in [here](https://github.com/MartenM/PixelPilot/tree/main/examples/Example.StructuresBot).