namespace Shopway.Tests.Performance.Options;

public sealed class PerformanceTestOptions
{
    public int RequestsPerSecond { get; set; }
    public int DurationInMinutes { get; set; }
    public int WarmUpDurationInSeconds { get; set; }
}