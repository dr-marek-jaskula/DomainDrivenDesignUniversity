using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Shopway.App.Options;

public class BearerAuthenticationOptionsSetup : IPostConfigureOptions<JwtBearerOptions>
{
    private readonly AuthenticationOptions _authenticationOptions;

    public BearerAuthenticationOptionsSetup(IOptions<AuthenticationOptions> authenticationOptions)
    {
        _authenticationOptions = authenticationOptions.Value;
    }

    public void PostConfigure(string? name, JwtBearerOptions options)
    {
        options.TokenValidationParameters.ValidIssuer = _authenticationOptions.Issuer;
        options.TokenValidationParameters.ValidAudience = _authenticationOptions.Audience;
        options.TokenValidationParameters.IssuerSigningKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationOptions.SecretKey));
    }
}