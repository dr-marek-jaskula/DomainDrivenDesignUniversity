using Shopway.Domain.Primitives;
using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Persistence.Specifications;

public abstract class BaseQuerySpecification<TEntity, TEntityId> : BaseSpecification<TEntity, TEntityId>
    where TEntityId : IEntityId, new()
    where TEntity : Entity<TEntityId>
{
	public BaseQuerySpecification()
	{
		IsAsNoTracking = true;
	}
}