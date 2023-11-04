using Shopway.Domain.Results;
using Shopway.Domain.EntityIds;
using Microsoft.AspNetCore.Authorization;

namespace Shopway.Presentation.Authentication.PermissionAuthentication;

public interface IPermissionService
{
    public Result<UserId> GetUserId(AuthorizationHandlerContext context);
    public Task<bool> HasPermissionAsync(UserId userId, string permission);
}