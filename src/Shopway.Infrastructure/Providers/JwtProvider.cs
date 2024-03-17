using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shopway.Application.Abstractions;
using Shopway.Application.Features.Users.Commands.LogUser;
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

    public AccessTokenResult GenerateJwt(User user)
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

        return new AccessTokenResult(accessToken, _options.AccessTokenExpirationInMinutes, refreshToken);
    }

    public Result<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            ValidateLifetime = false
        };

        var claimsPrincipal = new JwtSecurityTokenHandler()
            .ValidateToken(token, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, InvariantCultureIgnoreCase))
        {
            return Result.Failure<ClaimsPrincipal>(Error.InvalidArgument("Invalid token"));
        }

        return claimsPrincipal;
    }
}