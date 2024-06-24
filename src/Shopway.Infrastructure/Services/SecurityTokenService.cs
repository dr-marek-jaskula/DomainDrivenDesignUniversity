using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shopway.Application.Abstractions;
using Shopway.Application.Features.Users.Commands;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Users;
using Shopway.Domain.Users.ValueObjects;
using Shopway.Infrastructure.Options;
using Shopway.Infrastructure.Policies;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Google.Apis.Auth.GoogleJsonWebSignature;
using static Google.Apis.Requests.BatchRequest;
using static System.StringComparison;

namespace Shopway.Infrastructure.Services;

internal sealed class SecurityTokenService
(
    IOptions<AuthenticationOptions> authenticationOptions,
    IOptions<GoogleOptions> googleOptions,
    TimeProvider timeProvider
)
    : ISecurityTokenService
{
    private readonly AuthenticationOptions _options = authenticationOptions.Value;
    private readonly GoogleOptions _googleOptions = googleOptions.Value;
    private readonly TimeProvider _timeProvider = timeProvider;
    private const string _errorMessage = "Invalid token";

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
            signingCredentials: signingCredentials,
            notBefore: _timeProvider.GetLocalNow().DateTime
        );

        var accessToken = new JwtSecurityTokenHandler()
            .WriteToken(token);

        var refreshToken = RandomUtilities.GenerateString(RefreshToken.Length);

        return new AccessTokenResponse(accessToken, _options.AccessTokenExpirationInMinutes, refreshToken);
    }

    public Result<Claim?> GetClaimFromToken(string token, string claimInvariantName)
    {
        SecurityToken securityToken = GetSecurityToken(token);

        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, InvariantCultureIgnoreCase))
        {
            return Result.Failure<Claim?>(Error.InvalidArgument(_errorMessage));
        }

        return jwtSecurityToken.Claims.FirstOrDefault(x => x.Type.Equals(claimInvariantName, InvariantCultureIgnoreCase));
    }

    public Result<bool> HasRefreshTokenExpired(string token)
    {
        SecurityToken securityToken = GetSecurityToken(token);

        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, InvariantCultureIgnoreCase))
        {
            return Result.Failure<bool>(Error.InvalidArgument(_errorMessage));
        }

        return securityToken.ValidFrom.AddDays(_options.RefreshTokenExpirationInDays) < _timeProvider.GetUtcNow();
    }

    public bool HasTwoFactorTokenExpired(DateTimeOffset? twoFactorTokenCreatedOn)
    {
        if (twoFactorTokenCreatedOn is null)
        {
            return true;
        }

        return ((DateTimeOffset)twoFactorTokenCreatedOn).AddSeconds(_options.TwoFactorTokenExpirationInSeconds) < _timeProvider.GetUtcNow();
    }

    public Result<(Email Email, Username Username)> GetUserLogDetailsFormGoogleClaims(ClaimsPrincipal claimsPrincipal)
    {
        var givenNameClaimValue = claimsPrincipal.FindFirstValue(ClaimTypes.GivenName);
        var surnameClaimValue = claimsPrincipal.FindFirstValue(ClaimTypes.Surname);
        var emailClaimValue = claimsPrincipal.FindFirstValue(ClaimTypes.Email);

        if (surnameClaimValue is null | givenNameClaimValue is null || emailClaimValue is null)
        {
            return Result.Failure<(Email Email, Username Username)>(Error.InvalidArgument("Google user is missing at least one of required claims"));
        }

        var email = Email.Create(emailClaimValue);

        if (email.IsFailure)
        {
            return Result.Failure<(Email Email, Username Username)>(Error.InvalidArgument(email.Error));
        }

        var username = Username.Create($"{givenNameClaimValue}{surnameClaimValue}"); //Normalize

        if (username.IsFailure)
        {
            return Result.Failure<(Email Email, Username Username)>(Error.InvalidArgument(username.Error));
        }

        return Result.Success((email.Value, username.Value));
    }

    private SecurityToken GetSecurityToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            ValidateLifetime = false,
            ValidAudiences = [_options.Audience],
            ValidIssuers = [_options.Issuer]
        };

        new JwtSecurityTokenHandler()
            .ValidateToken(token, tokenValidationParameters, out SecurityToken? securityToken);

        return securityToken;
    }
}
