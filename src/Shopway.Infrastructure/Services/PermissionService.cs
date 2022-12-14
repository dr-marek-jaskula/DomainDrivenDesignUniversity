using Shopway.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shopway.Persistence.Framework;
using Shopway.Domain.StronglyTypedIds;
using Shopway.Infrastructure.Abstractions;

namespace Shopway.Infrastructure.Services;

public class PermissionService : IPermissionService
{
    private readonly ShopwayDbContext _context;

    public PermissionService(ShopwayDbContext context)
    {
        _context = context;
    }

    public async Task<HashSet<string>> GetPermissionsAsync(UserId userId)
    {
        ICollection<Role>[] roles = await _context.Set<User>()
            .Include(x => x.Roles)
            .ThenInclude(x => x.Permissions)
            .Where(x => x.Id == userId)
            .Select(x => x.Roles)
            .ToArrayAsync();

        return roles
            .SelectMany(x => x)
            .SelectMany(x => x.Permissions)
            .Select(x => x.Name)
            .ToHashSet();
    }
}
