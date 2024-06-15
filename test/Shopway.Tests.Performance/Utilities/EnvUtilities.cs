namespace Shopway.Tests.Performance.Utilities;

public static class EnvUtilities
{
    public static string GetEnvOrFallback(string fallback)
    {
        return Environment.GetEnvironmentVariable("ENVIRONMENT_NAME") ?? fallback;
    }

    public static string GetEnvOrLocal()
    {
        return GetEnvOrFallback("Local");
    }
}
