using Shopway.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shopway.Persistence.Framework;
using Shopway.Domain.EntityIds;
using Shopway.Persistence.Abstractions;
using Shopway.Domain.Abstractions.Repositories;

namespace Shopway.Persistence.Repositories;

public sealed class PermissionRepository : RepositoryBase, IPermissionRepository
{
    public PermissionRepository(ShopwayDbContext dbContext) : base(dbContext)
    {
    }

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

    public async Task<bool> HasPermissionAsync(UserId userId, string permissionName)
    {
        return await _dbContext
            .Set<User>()
            .Include(x => x.Roles)
                .ThenInclude(x => x.Permissions)
            .Where(x => x.Id == userId)
            .SelectMany(x => x.Roles)
            .SelectMany(role => role.Permissions)
            .Select(permission => permission.Name)
            .Where(name => name == permissionName)
            .AnyAsync();
    }
}
