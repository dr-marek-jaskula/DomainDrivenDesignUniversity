namespace Shopway.Tests.Performance.Utilities;

public sealed class SkipForLocalEnvFact : FactAttribute
{
    public SkipForLocalEnvFact()
    {
        var environment = Environment.GetEnvironmentVariable("PERFORMANCE_ENVIRONMENT");

        if (environment is null || environment.ToLower() is "local")
        {
            Skip = "Performance tests should not run on local environment";
        }
    }
}