namespace Shopway.Domain.Users;

public interface IPermissionRepository
{
    Task<HashSet<string>> GetPermissionsAsync(UserId userId);
    Task<bool> HasPermissionAsync(UserId userId, string permissionName);
}
