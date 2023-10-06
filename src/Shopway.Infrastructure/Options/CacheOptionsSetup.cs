using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Shopway.Infrastructure.Options;

public sealed class CacheOptionsSetup : IConfigureOptions<CacheOptions>
{
    private const string _configurationSectionName = "CacheConnection";
    private readonly IConfiguration _configuration;

    public CacheOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(CacheOptions options)
    {
        options.ConnectionString = _configuration
            .GetConnectionString(_configurationSectionName);
    }
}
