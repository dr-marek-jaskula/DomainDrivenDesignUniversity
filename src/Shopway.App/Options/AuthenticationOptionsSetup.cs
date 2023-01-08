using Microsoft.Extensions.Options;
using Shopway.Infrastructure.Authentication;

namespace Shopway.App.Options;

public class AuthenticationOptionsSetup : IConfigureOptions<AuthenticationOptions>
{
    private const string SectionName = "Authentication";
    private readonly IConfiguration _configuration;

    public AuthenticationOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(AuthenticationOptions options)
    {
        _configuration.GetSection(SectionName).Bind(options);
    }
}
