using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shopway.Application.Abstractions;
using Shopway.Application.Features.Users.Commands;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Errors;
using Shopway.Domain.Users;
using Shopway.Domain.Users.ValueObjects;
using Shopway.Infrastructure.Options;
using Shopway.Infrastructure.Policies;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.StringComparison;

namespace Shopway.Infrastructure.Providers;

internal sealed class JwtProvider(IOptions<AuthenticationOptions> options, TimeProvider timeProvider) : IJwtProvider
{
    private readonly AuthenticationOptions _options = options.Value;
    private readonly TimeProvider _timeProvider = timeProvider;

    public AccessTokenResponse GenerateJwt(User user)
    {
        var claims = new Claim[]
        {
            new(ClaimTypes.NameIdentifier, user.Id.Value.ToString()),
            new(JwtRegisteredClaimNames.Name, user.Username.Value),
            new(JwtRegisteredClaimNames.Email, user.Email.Value),
            new(ClaimPolicies.CustomerId, user switch
            {
                { CustomerId: not null } => $"{user.CustomerId}",
                _ => string.Empty
            })
        };

        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));

        var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

        var expires = _timeProvider.GetLocalNow().AddMinutes(_options.AccessTokenExpirationInMinutes);

        var token = new JwtSecurityToken
        (
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: expires.DateTime,
            signingCredentials: signingCredentials
        );

        var accessToken = new JwtSecurityTokenHandler()
            .WriteToken(token);

        var refreshToken = RandomUtilities.GenerateString(RefreshToken.Length);

        return new AccessTokenResponse(accessToken, _options.AccessTokenExpirationInMinutes, refreshToken);
    }

    public Result<Claim?> GetClaimFromExpiredToken(string token, string claimInvariantName)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            ValidateLifetime = false,
            ValidAudiences = [_options.Audience ],
            ValidIssuers = [ _options.Issuer ]
        };

        ClaimsPrincipal claimsPrincipal = new JwtSecurityTokenHandler()
            .ValidateToken(token, tokenValidationParameters, out SecurityToken? securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, InvariantCultureIgnoreCase))
        {
            return Result.Failure<Claim?>(Error.InvalidArgument("Invalid token"));
        }

        return jwtSecurityToken.Claims.FirstOrDefault(x => x.Type.ToLower() == claimInvariantName.ToLower());
    }
}