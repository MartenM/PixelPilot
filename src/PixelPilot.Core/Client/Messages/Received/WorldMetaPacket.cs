namespace PixelPilot.Client.Messages.Received;

public class WorldMetaPacket : IPixelGamePacket
{
    public WorldMetaPacket(string name, int plays, string owner, string description, string visibility, bool isUnsaved, bool hasUnsavedChanges)
    {
        Name = name;
        Plays = plays;
        Owner = owner;
        Description = description;
        Visibility = visibility;
        IsUnsaved = isUnsaved;
        HasUnsavedChanges = hasUnsavedChanges;
    }

    public string Name { get; }
    public int Plays { get; }
    public string Owner { get; }
    
    public string Description { get; }
    public string Visibility { get; }
    public bool IsUnsaved { get; }
    public bool HasUnsavedChanges { get; }
}