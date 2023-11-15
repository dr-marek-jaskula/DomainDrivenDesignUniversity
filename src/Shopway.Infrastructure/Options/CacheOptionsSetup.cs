using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Shopway.Infrastructure.Options;

internal sealed class CacheOptionsSetup(IConfiguration configuration) : IConfigureOptions<CacheOptions>
{
    private const string _configurationSectionName = "CacheConnection";
    private readonly IConfiguration _configuration = configuration;

    public void Configure(CacheOptions options)
    {
        options.ConnectionString = _configuration
            .GetConnectionString(_configurationSectionName);
    }
}
