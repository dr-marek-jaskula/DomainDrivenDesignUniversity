using Microsoft.AspNetCore.Authorization;
using Shopway.Domain.Common.Enums;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Users;

namespace Shopway.Presentation.Authentication.RolePermissionAuthentication;

public interface IUserAuthorizationService<TPermission, TRole>
    where TPermission : struct, Enum
    where TRole : struct, Enum
{
    Result<UserId> GetUserId(AuthorizationHandlerContext context);
    Task<bool> HasPermissionsAsync(UserId userId, LogicalOperation logicalOperation, params TPermission[] permissions);
    Task<bool> HasPermissionToReadAsync(UserId userId, string entity, List<string> requestedProperties);
    Task<bool> HasRolesAsync(UserId userId, params TRole[] roles);
}
