using Shopway.Domain.Users.Enumerations;
using Shopway.Domain.Users.ValueObjects;

namespace Shopway.Domain.Users;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken);

    Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken);

    Task<User?> GetByUsernameAsync(Username username, CancellationToken cancellationToken);

    Task<Role?> GetRolePermissionsAsync(Role role, CancellationToken cancellationToken);

    Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken);

    Task<bool> IsEmailTakenAsync(Email email, CancellationToken cancellationToken);

    void Add(User user);

    void Update(User user);
}