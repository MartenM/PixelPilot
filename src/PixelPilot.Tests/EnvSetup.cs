namespace PixelGameTests;

/// <summary>
/// Loads a local ".env" file (if present next to the test binaries) into the process
/// environment before any tests run, so PIXELWALKER_TEST_EMAIL / PIXELWALKER_TEST_PASSWORD
/// (and anything else) don't need to be exported manually for local runs.
/// Variables already present in the real environment (e.g. CI secrets) are never overridden.
/// </summary>
[SetUpFixture]
public class EnvSetup
{
    [OneTimeSetUp]
    public void LoadDotEnv()
    {
        var envPath = Path.Combine(TestContext.CurrentContext.TestDirectory, ".env");
        if (!File.Exists(envPath)) return;

        foreach (var rawLine in File.ReadAllLines(envPath))
        {
            var line = rawLine.Trim();
            if (line.Length == 0 || line.StartsWith('#')) continue;

            var separatorIndex = line.IndexOf('=');
            if (separatorIndex <= 0) continue;

            var key = line[..separatorIndex].Trim();
            var value = line[(separatorIndex + 1)..].Trim().Trim('"');

            // Don't override variables already set in the real environment (e.g. CI secrets).
            if (Environment.GetEnvironmentVariable(key) != null) continue;

            Environment.SetEnvironmentVariable(key, value);
        }
    }
}
