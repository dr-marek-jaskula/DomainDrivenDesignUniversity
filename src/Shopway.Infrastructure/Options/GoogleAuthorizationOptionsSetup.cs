using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Shopway.Infrastructure.Options;

internal sealed class GoogleAuthorizationOptionsSetup(IConfiguration configuration) : IConfigureOptions<GoogleAuthorizationOptions>
{
    private const string _configurationSectionName = "GoogleAuthorizationOptions";
    private readonly IConfiguration _configuration = configuration;

    public void Configure(GoogleAuthorizationOptions options)
    {
        _configuration
            .GetSection(_configurationSectionName)
            .Bind(options);
    }
}
