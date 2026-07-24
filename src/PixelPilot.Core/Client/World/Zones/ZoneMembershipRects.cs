using System.Drawing;

namespace PixelPilot.Client.World.Zones;

/// <summary>
/// Decomposes a boolean membership mask into rectangles, for use with
/// WorldZoneAreaEditRequestPacket — the only way a client can edit zone membership; the server
/// ignores any membership_rle sent on a zone upsert. Adjacent rows with an identical run pattern
/// are merged into one taller rectangle; otherwise each contiguous horizontal run of `true` cells
/// becomes its own rectangle.
/// </summary>
public static class ZoneMembershipRects
{
    public static List<Rectangle> Decompose(bool[,] mask)
    {
        var width = mask.GetLength(0);
        var height = mask.GetLength(1);

        var rects = new List<Rectangle>();
        var active = new List<(int X, int Width, int StartY)>();

        for (var y = 0; y <= height; y++)
        {
            var runs = y < height ? GetRowRuns(mask, y, width) : new List<(int X, int Width)>();

            if (RunsMatch(active, runs)) continue;

            foreach (var a in active)
            {
                rects.Add(new Rectangle(a.X, a.StartY, a.Width, y - a.StartY));
            }

            active = runs.Select(r => (r.X, r.Width, y)).ToList();
        }

        return rects;
    }

    /// <summary>
    /// Compares a current membership mask against a target one and splits the difference into
    /// rectangles to add and rectangles to remove. The two masks may differ in size (out-of-range
    /// cells are treated as not-a-member).
    /// </summary>
    public static (List<Rectangle> Add, List<Rectangle> Remove) Diff(bool[,] current, bool[,] target)
    {
        var width = target.GetLength(0);
        var height = target.GetLength(1);
        var currentWidth = current.GetLength(0);
        var currentHeight = current.GetLength(1);

        var add = new bool[width, height];
        var remove = new bool[width, height];

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                var wasIn = x < currentWidth && y < currentHeight && current[x, y];
                var isIn = target[x, y];

                if (isIn && !wasIn) add[x, y] = true;
                else if (!isIn && wasIn) remove[x, y] = true;
            }
        }

        return (Decompose(add), Decompose(remove));
    }

    private static List<(int X, int Width)> GetRowRuns(bool[,] mask, int y, int width)
    {
        var runs = new List<(int X, int Width)>();
        var x = 0;
        while (x < width)
        {
            if (!mask[x, y])
            {
                x++;
                continue;
            }

            var start = x;
            while (x < width && mask[x, y]) x++;
            runs.Add((start, x - start));
        }

        return runs;
    }

    private static bool RunsMatch(List<(int X, int Width, int StartY)> active, List<(int X, int Width)> runs)
    {
        if (active.Count != runs.Count) return false;

        for (var i = 0; i < active.Count; i++)
        {
            if (active[i].X != runs[i].X || active[i].Width != runs[i].Width) return false;
        }

        return true;
    }
}
