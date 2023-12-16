using Shopway.Domain.Enums;

namespace Shopway.Domain.Users;

public interface IAuthorizationRepository
{
    Task<HashSet<string>> GetPermissionsAsync(UserId userId);
    Task<bool> HasPermissionsAsync(UserId userId, Permission[] permissions);
    Task<bool> HasRolesAsync(UserId userId, Role[] roles);
}