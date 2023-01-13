using Microsoft.Extensions.Options;

namespace Shopway.App.Options;

public sealed class CacheOptionsSetup : IConfigureOptions<CacheOptions>
{
    private readonly IConfiguration _configuration;

    public CacheOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(CacheOptions options)
    {
        options.ConnectionString = _configuration
            .GetConnectionString("CacheConnection");
    }
}
