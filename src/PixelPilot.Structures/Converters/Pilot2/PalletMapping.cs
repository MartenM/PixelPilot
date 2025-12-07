namespace PixelPilot.Structures.Converters.Pilot2;

/// <summary>
/// Used for saving PixelWalker blocks.
/// </summary>
public class PalletMapping
{
    public required string Name { get; set; }
    public required Dictionary<string, object> Fields { get; set; }

    protected bool Equals(PalletMapping other)
    {
        return Name == other.Name && EqualFields(other);
    }

    private bool EqualFields(PalletMapping other)
    {
        return Fields.Count == other.Fields.Count
               && Fields.All(kvp => other.Fields.TryGetValue(kvp.Key, out var value) && kvp.Value.Equals(value));
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((PalletMapping)obj);
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Name);

        foreach (var kvp in Fields.OrderBy(k => k.Key))
        {
            hash.Add(kvp.Key);
            hash.Add(kvp.Value);
        }

        return hash.ToHashCode();
    }
    
}