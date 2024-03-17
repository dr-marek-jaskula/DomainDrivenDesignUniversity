using Shopway.Application.Features.Users.Commands;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Users;
using System.Security.Claims;

namespace Shopway.Application.Abstractions;

public interface IJwtProvider
{
    AccessTokenResponse GenerateJwt(User user);
    Result<Claim?> GetClaimFromExpiredToken(string token, string claimInvariantName);
}