using Shopway.Application.Features.Proxy.GenericValidators;
using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.DataProcessing.Proxy;

namespace Shopway.Application.Features.Proxy.GenericPageQuery;

internal sealed class GenericCursorPageQueryValidator<TEntity, TEntityId>
    : GenericCursorPageQueryValidator<GenericCursorPageQuery<TEntity, TEntityId>, DataTransferObjectResponse, DynamicFilter<TEntity, TEntityId>, DynamicSortBy<TEntity, TEntityId>, DynamicMapping<TEntity, TEntityId>, CursorPage>
    where TEntity : Entity<TEntityId>
    where TEntityId : struct, IEntityId<TEntityId>
{
    public GenericCursorPageQueryValidator() : base()
    {
    }
}
