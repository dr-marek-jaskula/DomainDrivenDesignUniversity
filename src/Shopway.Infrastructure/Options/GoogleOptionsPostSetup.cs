using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.Options;

namespace Shopway.Infrastructure.Options;

internal sealed class GoogleOptionsPostSetup(IOptions<GoogleAuthorizationOptions> authenticationOptions)
    : IPostConfigureOptions<GoogleOptions>
{
    private readonly GoogleAuthorizationOptions _authenticationOptions = authenticationOptions.Value;

    public void PostConfigure(string? name, GoogleOptions options)
    {
        options.ClientId = _authenticationOptions.ClientId;
        options.ClientSecret = _authenticationOptions.ClientSecret;
    }
}
