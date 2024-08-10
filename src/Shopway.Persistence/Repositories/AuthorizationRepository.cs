using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Common.Enums;
using Shopway.Domain.Users;
using Shopway.Domain.Users.Authorization;
using Shopway.Persistence.Framework;

namespace Shopway.Persistence.Repositories;

internal sealed class AuthorizationRepository<TPermission, TRole>(ShopwayDbContext dbContext) : IAuthorizationRepository<TPermission, TRole>
    where TPermission : struct, Enum
    where TRole : struct, Enum
{
    private readonly ShopwayDbContext _dbContext = dbContext;

    public async Task<HashSet<string>> GetPermissionsAsync(UserId userId)
    {
        var permissions = await _dbContext
            .Set<User>()
            .Where(x => x.Id == userId)
            .SelectMany(x => x.Roles)
            .SelectMany(role => role.Permissions)
            .Select(permission => permission.Name)
            .ToArrayAsync();

        return permissions
            .ToHashSet();
    }

    public async Task<Permission?> GetPermissionAsync(TPermission permission, CancellationToken cancellationToken)
    {
        var permissionName = $"{permission}";

        return await _dbContext
            .Set<Permission>()
            .Where(x => x.Name == permissionName)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> HasPermissionsAsync(UserId userId, TPermission[] requiredPermissions, LogicalOperation logicalOperation = LogicalOperation.And)
    {
        var distinctRequiredPermissions = requiredPermissions
            .Select(x => $"{x}")
            .Distinct()
            .ToArray();

        var userPermissionsQueryable = _dbContext
            .Set<User>()
            .Where(x => x.Id == userId)
            .SelectMany(x => x.Roles)
            .SelectMany(role => role.Permissions)
            .Select(permission => permission.Name)
            .Where(permissionName => distinctRequiredPermissions.Contains(permissionName))
            .Distinct();

        if (logicalOperation is LogicalOperation.And)
        {
            var userPermissions = await userPermissionsQueryable
                .CountAsync();

            return userPermissions == distinctRequiredPermissions.Length;
        }

        if (logicalOperation is LogicalOperation.Or)
        {
            return await userPermissionsQueryable.AnyAsync();
        }

        return false;
    }

    public async Task<bool> HasPermissionToReadAsync(UserId userId, string entity, List<string> requestedProperties)
    {
        var distinctRequiredPermissions = requestedProperties
            .Distinct()
            .ToArray();

        return await _dbContext
            .Set<User>()
            .Where(x => x.Id == userId)
            .SelectMany(x => x.Roles)
            .SelectMany(role => role.Permissions)
            .Where(permission => permission.RelatedEntity == entity)
            .Where(permission => permission.Type == PermissionType.Read)
            .Where(permission => permission.Properties == null || !requestedProperties.Except(permission.Properties).Any())
            .AnyAsync();
    }

    public async Task<bool> HasRolesAsync(UserId userId, TRole[] requiredRoles)
    {
        var distinctRequiredRoles = requiredRoles
            .Select(x => $"{x}")
            .Distinct()
            .ToArray();

        var userRolesCount = await _dbContext
            .Set<User>()
            .Where(x => x.Id == userId)
            .SelectMany(user => user.Roles)
            .Where(role => distinctRequiredRoles.Contains(role.Name))
            .Distinct()
            .CountAsync();

        return userRolesCount == distinctRequiredRoles.Length;
    }

    public async Task<Role?> GetRolePermissionsAsync(TRole role, CancellationToken cancellationToken)
    {
        var roleName = $"{role}";

        return await _dbContext
            .Set<Role>()
                .Include(x => x.Permissions)
            .Where(x => x.Name == roleName)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task CreatePermissionAsync(Permission permission)
    {
        _dbContext
            .Set<Permission>()
            .Add(permission);

        return Task.CompletedTask;
    }

    public Task DeletePermissionAsync(Permission permission)
    {
        _dbContext
            .Set<Permission>()
            .Remove(permission);

        return Task.CompletedTask;
    }
}
