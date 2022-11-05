using Shopway.Domain.Primitives;
using Shopway.Persistence.Specifications;

namespace Shopway.Persistence.Repositories;

public abstract class BaseRepository
{
    protected readonly ApplicationDbContext _dbContext;

    protected BaseRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected IQueryable<TEntity> ApplySpecification<TEntity>(Specification<TEntity> specification) 
        where TEntity : Entity
    {
        return SpecificationEvaluator.GetQuery(_dbContext.Set<TEntity>(), specification);
    }
}