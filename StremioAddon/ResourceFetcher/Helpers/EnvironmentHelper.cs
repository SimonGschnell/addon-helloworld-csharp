namespace ResourceFetcher.Helpers;

public static class EnvironmentHelper
{
    public static string GetOutputPath()
    {
        var outputPath = Environment.GetEnvironmentVariable("RESOURCE_FETCHER_OUTPUT_PATH");
        if (string.IsNullOrEmpty(outputPath))
        {
            throw new Exception("RESOURCE_FETCHER_OUTPUT_PATH env not set");
        }

        return outputPath;
    }
}