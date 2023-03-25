using Shopway.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shopway.Persistence.Framework;
using Shopway.Domain.EntityIds;
using Shopway.Infrastructure.Abstractions;
using Shopway.Domain.Enumerations;

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
        IReadOnlyCollection<Role>[] roles = await _context
            .Set<User>()
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
