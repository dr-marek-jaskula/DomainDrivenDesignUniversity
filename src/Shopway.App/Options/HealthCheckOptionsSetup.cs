using Microsoft.Extensions.Options;

namespace Shopway.App.Options;

public sealed class HealthCheckOptionsSetup : IConfigureOptions<HealthOptions>
{
    private readonly string _configurationSectionName = "HealthOptions";
    private readonly IConfiguration _configuration;

    public HealthCheckOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(HealthOptions options)
    {
        _configuration
            .GetSection(_configurationSectionName)
            .Bind(options);
    }
}