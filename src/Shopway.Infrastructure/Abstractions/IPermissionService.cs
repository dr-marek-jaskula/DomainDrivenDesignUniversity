using Shopway.Domain.EntityIds;

namespace Shopway.Infrastructure.Abstractions;

public interface IPermissionService
{
    Task<HashSet<string>> GetPermissionsAsync(UserId userId);
}
