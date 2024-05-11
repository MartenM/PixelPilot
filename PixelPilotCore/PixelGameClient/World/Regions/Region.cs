using PixelPilot.PixelGameClient.World.Blocks;

namespace PixelPilot.PixelGameClient.World.Regions;

/// <summary>
/// Regions represents an area of blocks. Regions have a Name and Author property.
/// An additional version int is also encoded which might be useful in the future if block conversions happen.
///
/// The blocks don't keep their original position but are shifted to 0,0.
/// </summary>
public class Region
{
    public string Name { get; set; }
    public string Author { get; set; }
    
    public int Version { get; set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    
    public IPixelBlock[,,] Blocks;
    
    public Region(string name, string author, IPixelBlock[,,] blocks)
    {
        Name = name;
        Author = author;
        Width = blocks.GetLength(1);
        Height = blocks.GetLength(2);
        Blocks = blocks;
    }
}