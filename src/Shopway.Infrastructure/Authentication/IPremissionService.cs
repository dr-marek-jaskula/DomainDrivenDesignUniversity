using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Infrastructure.Authentication;

public interface IPermissionService
{
    Task<HashSet<string>> GetPermissionsAsync(UserId userId);
}
