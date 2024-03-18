using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Common.Enums;
using Shopway.Domain.Enums;
using Shopway.Domain.Users;
using Shopway.Persistence.Framework;

namespace Shopway.Persistence.Repositories;

public sealed class AuthorizationRepository(ShopwayDbContext dbContext) : IAuthorizationRepository
{
    private readonly ShopwayDbContext _dbContext = dbContext;

    public async Task<HashSet<string>> GetPermissionsAsync(UserId userId)
    {
        var permissions = await _dbContext
            .Set<User>()
            .Include(x => x.Roles)
                .ThenInclude(x => x.Permissions)
            .Where(x => x.Id == userId)
            .SelectMany(x => x.Roles)
            .SelectMany(role => role.Permissions)
            .Select(permission => permission.Name)
            .ToArrayAsync();

        return permissions
            .ToHashSet();
    }

    public async Task<bool> HasPermissionsAsync(UserId userId, Permission[] requiredPermissions, LogicalOperation logicalOperation = LogicalOperation.And)
    {
        var distinctRequiredPermissions = requiredPermissions
            .Select(x => $"{x}")
            .Distinct()
            .ToArray();

        var userPermissionsQueryable = _dbContext
            .Set<User>()
            .Include(x => x.Roles)
                .ThenInclude(x => x.Permissions)
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

    public async Task<bool> HasRolesAsync(UserId userId, Role[] requiredRoles)
    {
        var distinctRequiredRoles = requiredRoles
            .Select(x => $"{x}")
            .Distinct()
            .ToArray();

        var userRolesCount = await _dbContext
            .Set<User>()
            .Include(x => x.Roles)
            .Where(x => x.Id == userId)
            .SelectMany(user => user.Roles)
            .Where(role => distinctRequiredRoles.Contains(role.Name))
            .Distinct()
            .CountAsync();

        return userRolesCount == distinctRequiredRoles.Length;
    }
}
