using Shopway.Application.Features.Users.Commands;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Users;
using Shopway.Domain.Users.ValueObjects;
using System.Security.Claims;

namespace Shopway.Application.Abstractions;

public interface ISecurityTokenService
{
    AccessTokenResponse GenerateJwt(User user);
    Result<Claim?> GetClaimFromToken(string token, string claimInvariantName);
    Result<(Email Email, Username Username)> GetUserLogDetailsFormGoogleClaims(ClaimsPrincipal claimsPrincipal);
    Result<bool> HasRefreshTokenExpired(string token);
    bool HasTwoFactorTokenExpired(DateTimeOffset? twoFactorTokenCreatedOn);
}
