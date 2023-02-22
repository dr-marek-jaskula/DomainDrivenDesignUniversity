using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Shopway.Infrastructure.Options;

public sealed class AuthenticationOptionsSetup : IConfigureOptions<AuthenticationOptions>
{
    private readonly string _configurationSectionName = "AuthenticationOptions";
    private readonly IConfiguration _configuration;

    public AuthenticationOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(AuthenticationOptions options)
    {
        _configuration
            .GetSection(_configurationSectionName)
            .Bind(options);
    }
}
