using Shopway.Application.Features.Users.Commands;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Users;
using System.Security.Claims;

namespace Shopway.Application.Abstractions;

public interface IJwtProvider
{
    AccessTokenResponse GenerateJwt(User user);
    Result<Claim?> GetClaimFromToken(string token, string claimInvariantName);
    Result<bool> HasRefreshTokenExpired(string token);
    bool HasTwoFactorTokenExpired(DateTimeOffset? twoFactorTokenCreatedOn);
}