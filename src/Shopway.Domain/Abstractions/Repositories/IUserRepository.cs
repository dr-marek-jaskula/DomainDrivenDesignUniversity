using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Abstractions.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken);

    Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken);

    Task<User?> GetByUsernameAsync(Username username, CancellationToken cancellationToken);

    Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken);

    Task<bool> IsEmailTakenAsync(Email email, CancellationToken cancellationToken);

    void Add(User user);

    void Update(User user);
}