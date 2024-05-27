# Structures
Structures are used to load and save blocks.

### Saved format
The saved format tries to be as compatible as possible with the original game. However, the names of blocks are subject to change.
This means that to remain compatible a version tag was included.


#### Binary array format
```
version: int
width: int
height: int
meta-tag-count: int
meta-tags: Array<(string, string)>
meta-contains-air: bool
mapping-count: int
mappings: Array<string>
data-compression: int
data: like block packet out, but changed ID to mapping
```

#### JSON Format
```json
{
  "version": 0,
  "width": 0,
  "height": 0,
  "meta-tags": {
    "key": "value"
  },
  "contains-air": false,
  "mapping": ["empty", "air"],
  "data-format": "naive",
  "data": "<DATA>"
}
```