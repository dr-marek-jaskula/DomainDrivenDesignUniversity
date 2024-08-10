using Shopway.Domain.Common.Enums;

namespace Shopway.Domain.Users.Authorization;

public interface IAuthorizationRepository<TPermission, TRole>
    where TPermission : struct, Enum
    where TRole : struct, Enum
{
    Task CreatePermissionAsync(Permission value);
    Task DeletePermissionAsync(Permission permission);
    Task<Permission?> GetPermissionAsync(TPermission permission, CancellationToken cancellationToken);
    Task<HashSet<string>> GetPermissionsAsync(UserId userId);
    Task<Role?> GetRolePermissionsAsync(TRole role, CancellationToken cancellationToken);
    Task<bool> HasPermissionToReadAsync(UserId userId, string entity, List<string> requestedProperties);
    Task<bool> HasPermissionsAsync(UserId userId, TPermission[] requiredPermissions, LogicalOperation logicalOperation = LogicalOperation.And);
    Task<bool> HasRolesAsync(UserId userId, TRole[] roles);
}
