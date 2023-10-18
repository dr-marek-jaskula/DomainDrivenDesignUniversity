using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Domain.ValueObjects;
using Shopway.Persistence.Framework;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Abstractions.Repositories;

namespace Shopway.Persistence.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly ShopwayDbContext _dbContext;

    public UserRepository(ShopwayDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken)
    {
        return await _dbContext
            .Set<User>()
            .Where(user => user.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken)
    {
        return await _dbContext
            .Set<User>()
            .Where(user => user.Email == email)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken)
    {
        return await IsEmailTakenAsync(email, cancellationToken) is false;
    }

    public async Task<bool> IsEmailTakenAsync(Email email, CancellationToken cancellationToken)
    {
        return await _dbContext
            .Set<User>()
            .Where(user => user.Email == email)
            .AnyAsync(cancellationToken);
    }

    public void Add(User user)
    {
        _dbContext
            .Set<User>()
            .Add(user);
    }

    public void Update(User user)
    {
        _dbContext
            .Set<User>()
            .Update(user);
    }

    public async Task<User?> GetByUsernameAsync(Username username, CancellationToken cancellationToken)
    {
        return await _dbContext
            .Set<User>()
            .Where(user => user.Username == username)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
