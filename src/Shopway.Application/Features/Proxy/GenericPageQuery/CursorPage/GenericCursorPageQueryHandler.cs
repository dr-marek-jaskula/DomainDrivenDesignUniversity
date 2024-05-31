using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.DataProcessing.Proxy;
using Shopway.Domain.Common.Results;

namespace Shopway.Application.Features.Proxy.GenericPageQuery;

internal sealed class GenericCursorPageQueryHandler<TEntity, TEntityId>(IProxyRepository<TEntity, TEntityId> proxyRepository)
    : ICursorPageQueryHandler<GenericCursorPageQuery<TEntity, TEntityId>, DataTransferObjectResponse, DynamicFilter<TEntity, TEntityId>, DynamicSortBy<TEntity, TEntityId>, DynamicMapping<TEntity, TEntityId>, CursorPage>
    where TEntity : Entity<TEntityId>
    where TEntityId : struct, IEntityId<TEntityId>
{
    private readonly IProxyRepository<TEntity, TEntityId> _proxyRepository = proxyRepository;

    public async Task<IResult<CursorPageResponse<DataTransferObjectResponse>>> Handle(GenericCursorPageQuery<TEntity, TEntityId> pageQuery, CancellationToken cancellationToken)
    {
        var page = await _proxyRepository
            .PageAsync(pageQuery.Page, cancellationToken, filter: pageQuery.Filter, sort: pageQuery.SortBy, mapping: pageQuery.Mapping);

        return page
            .ToPageResponse(pageQuery.Page)
            .ToResult();
    }
}
