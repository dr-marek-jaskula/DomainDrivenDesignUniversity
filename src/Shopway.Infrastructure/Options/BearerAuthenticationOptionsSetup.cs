using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Shopway.Infrastructure.Options;

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
        options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationOptions.SecretKey));
    }
}