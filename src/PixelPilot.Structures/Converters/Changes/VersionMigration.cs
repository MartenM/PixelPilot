using PixelPilot.Structures.Converters.PilotSimple;

namespace PixelPilot.Structures.Converters.Migrations;

public abstract class VersionMigration(int fromVersion)
{
    public int FromVersion { get; private set; } = fromVersion;
    public int ToVersion => FromVersion + 1;

    public void ApplyMigration(MappedBlockData mappedBlockData)
    {
        DoUpdate(mappedBlockData);
        mappedBlockData.Version = ToVersion;
    }

    protected abstract void DoUpdate(MappedBlockData mappedBlockData);
}