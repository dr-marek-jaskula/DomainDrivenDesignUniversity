using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace Shopway.Infrastructure.Options;

internal sealed class AuthenticationOptionsSetup(IConfiguration configuration) : IConfigureOptions<AuthenticationOptions>
{
    private const string _configurationSectionName = "AuthenticationOptions";
    private readonly IConfiguration _configuration = configuration;

    public void Configure(AuthenticationOptions options)
    {
        _configuration
            .GetSection(_configurationSectionName)
            .Bind(options);
    }
}
