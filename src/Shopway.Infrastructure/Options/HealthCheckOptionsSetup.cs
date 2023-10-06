using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Shopway.Infrastructure.Options;

public sealed class HealthCheckOptionsSetup : IConfigureOptions<HealthOptions>
{
    private const string _configurationSectionName = "HealthOptions";
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