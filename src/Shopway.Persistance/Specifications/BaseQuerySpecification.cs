using Shopway.Domain.Primitives;
using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Persistence.Specifications;

//This is commented due to the use of the QueryTransactionPipeline (Application -> Pipelines -> QueryPipelines) which is in my opinion a better approach
//Nevertheless this approach is still a good way, so choose your own preferred methodology
//public abstract class BaseQuerySpecification<TEntity, TEntityId> : BaseSpecification<TEntity, TEntityId>
//    where TEntityId : IEntityId<TEntityId>, new()
//    where TEntity : Entity<TEntityId>
//{
//	public BaseQuerySpecification()
//	{
//		IsAsNoTracking = true;
//	}
//}