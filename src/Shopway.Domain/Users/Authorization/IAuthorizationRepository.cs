using Shopway.Domain.Common.Enums;

namespace Shopway.Domain.Users.Authorization;

public interface IAuthorizationRepository
{
    Task<HashSet<string>> GetPermissionsAsync(UserId userId);
    Task<bool> HasPermissionsAsync(UserId userId, Permission[] requiredPermissions, LogicalOperation logicalOperation = LogicalOperation.And);
    Task<bool> HasRolesAsync(UserId userId, Role[] roles);
}
