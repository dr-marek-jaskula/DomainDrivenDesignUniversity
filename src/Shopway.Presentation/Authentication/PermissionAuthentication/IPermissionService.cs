using Shopway.Domain.Users;
using Shopway.Domain.Common.Results;
using Microsoft.AspNetCore.Authorization;

namespace Shopway.Presentation.Authentication.PermissionAuthentication;

public interface IPermissionService
{
    public Result<UserId> GetUserId(AuthorizationHandlerContext context);
    public Task<bool> HasPermissionAsync(UserId userId, string permission);
}