namespace Shopway.Tests.Performance.Utilities;

public sealed class SkipForLocalEnvFact : FactAttribute
{
    public SkipForLocalEnvFact()
    {
        var environment = EnvUtilities.GetEnvOrLocal();

        if (environment.Equals("Local", StringComparison.InvariantCultureIgnoreCase))
        {
            Skip = "Performance tests should not run on local environment";
        }
    }
}