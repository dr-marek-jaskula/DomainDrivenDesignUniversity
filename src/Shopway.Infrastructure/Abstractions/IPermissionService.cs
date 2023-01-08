using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Infrastructure.Abstractions;

public interface IPermissionService
{
    Task<HashSet<string>> GetPermissionsAsync(UserId userId);
}
