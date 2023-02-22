namespace Shopway.Infrastructure.Options;

//This is an object to which we will map the settings from the appsettings.json (from DatabaseOptions section)
//The mapping and binding will be done using "DatabaseOptionsSetup" class
public sealed class DatabaseOptions
{
    public string? ConnectionString { get; set; } = string.Empty;
    public int MaxRetryCount { get; set; }
    public int MaxRetryDelay { get; set; }
    public int CommandTimeout { get; set; }
}