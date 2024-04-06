using NBomber.Contracts;
using NBomber.Contracts.Stats;
using NBomber.CSharp;

namespace Shopway.Tests.Performance.Utilities;

public static class NBomberUtilities
{
    private static readonly ReportFormat[] _reportFormats = [ ReportFormat.Html ];
    private static readonly TimeSpan _fiveSecondsReportingInterval = TimeSpan.FromSeconds(5);

    public static NBomberContext WithReport(this NBomberContext context, string suiteType, string testName)
    {
        var environment = EnvUtilities.GetEnvOrLocal();

        return context
            .WithTestSuite($"{environment}_{suiteType}")
            .WithTestName($"{environment}_{testName}")
            .WithReportFolder(Constants.Constants.NBomber.Reports)
            .WithReportFileName($"{Constants.Constants.NBomber.Report}_{environment}_{suiteType}")
            .WithReportFormats(_reportFormats)
            .WithReportingInterval(_fiveSecondsReportingInterval);
    }

    public static ScenarioProps WithSetup(this ScenarioProps scenarioProps, LoadSimulation loadSimulation, TimeSpan warmUpDuration)
    {
        return scenarioProps
            .WithWarmUpDuration(warmUpDuration)
            .WithLoadSimulations(loadSimulation);
    }
}