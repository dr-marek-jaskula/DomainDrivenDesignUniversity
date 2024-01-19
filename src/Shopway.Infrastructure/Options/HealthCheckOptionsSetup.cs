using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Shopway.Infrastructure.Options;

internal sealed class HealthCheckOptionsSetup(IConfiguration configuration) : IConfigureOptions<HealthOptions>
{
    private const string _configurationSectionName = "HealthOptions";
    private readonly IConfiguration _configuration = configuration;

    public void Configure(HealthOptions options)
    {
        _configuration
            .GetSection(_configurationSectionName)
            .Bind(options);
    }
}