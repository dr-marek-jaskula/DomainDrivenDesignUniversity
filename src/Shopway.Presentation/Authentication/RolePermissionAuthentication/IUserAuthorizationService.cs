using Microsoft.AspNetCore.Authorization;
using Shopway.Domain.Common.Enums;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Users;
using Shopway.Domain.Users.Authorization;

namespace Shopway.Presentation.Authentication.RolePermissionAuthentication;

public interface IUserAuthorizationService
{
    Result<UserId> GetUserId(AuthorizationHandlerContext context);
    Task<bool> HasPermissionsAsync(UserId userId, LogicalOperation logicalOperation, params PermissionName[] permissions);
    Task<bool> HasPermissionToReadAsync(UserId userId, string entity, List<string> requestedProperties);
    Task<bool> HasRolesAsync(UserId userId, params RoleName[] roles);
}
