using Shopway.Domain.EntityIds;

namespace Shopway.Domain.Abstractions.Repositories;

public interface IPermissionRepository
{
    Task<HashSet<string>> GetPermissionsAsync(UserId userId);
}
