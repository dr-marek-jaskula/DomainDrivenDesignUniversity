using Shopway.Domain.Users;
using Shopway.Domain.Common.Results;
using Microsoft.AspNetCore.Authorization;
using Shopway.Domain.Enums;

namespace Shopway.Presentation.Authentication.RolePermissionAuthentication;

public interface IUserAuthorizationService
{
    public Result<UserId> GetUserId(AuthorizationHandlerContext context);
    public Task<bool> HasPermissionsAsync(UserId userId, params Permission[] permissions);
    public Task<bool> HasRolesAsync(UserId userId, params Role[] roles);
}