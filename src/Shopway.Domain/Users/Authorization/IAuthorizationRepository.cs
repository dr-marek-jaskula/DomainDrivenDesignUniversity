using Shopway.Domain.Common.Enums;

namespace Shopway.Domain.Users.Authorization;

public interface IAuthorizationRepository
{
    Task CreatePermissionAsync(Permission value);
    Task DeletePermissionAsync(Permission permission);
    Task<Permission?> GetPermissionAsync(PermissionName permission, CancellationToken cancellationToken);
    Task<HashSet<string>> GetPermissionsAsync(UserId userId);
    Task<Role?> GetRolePermissionsAsync(RoleName role, CancellationToken cancellationToken);
    Task<bool> HasPermissionsAsync(UserId userId, PermissionName[] requiredPermissions, LogicalOperation logicalOperation = LogicalOperation.And);
    Task<bool> HasPermissionToReadAsync(UserId userId, string entity, List<string> requestedProperties);
    Task<bool> HasRolesAsync(UserId userId, RoleName[] roles);
}
