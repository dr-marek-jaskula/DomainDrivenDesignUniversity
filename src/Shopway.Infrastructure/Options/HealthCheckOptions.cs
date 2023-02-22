namespace Shopway.Infrastructure.Options;

public sealed class HealthOptions
{
    public int DelayInSeconds { get; set; }
    public int PeriodInSeconds { get; set; }
}
