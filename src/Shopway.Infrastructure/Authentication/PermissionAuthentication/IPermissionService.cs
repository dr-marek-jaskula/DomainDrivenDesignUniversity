using Microsoft.AspNetCore.Authorization;
using Shopway.Domain.EntityIds;
using Shopway.Domain.Results;

namespace Shopway.Infrastructure.Authentication.PermissionAuthentication;

public interface IPermissionService
{
    public Result<UserId> GetUserId(AuthorizationHandlerContext context);
    public Task<bool> HasPermissionAsync(UserId userId, string permission);
}