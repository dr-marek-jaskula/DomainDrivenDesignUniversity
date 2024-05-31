using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.DataProcessing.Proxy;
using Shopway.Domain.Common.Results;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.DataProcessing;

namespace Shopway.Application.Features.Proxy;

internal sealed class GenericOffsetPageQueryHandler<TEntity, TEntityId>(IProxyRepository<TEntity, TEntityId> proxyRepository)
    : IOffsetPageQueryHandler<GenericOffsetPageQuery<TEntity, TEntityId>, DataTransferObjectResponse, DynamicFilter<TEntity, TEntityId>, DynamicSortBy<TEntity, TEntityId>, DynamicMapping<TEntity, TEntityId>, OffsetPage>
    where TEntity : Entity<TEntityId>
    where TEntityId : struct, IEntityId<TEntityId>
{
    private readonly IProxyRepository<TEntity, TEntityId> _proxyRepository = proxyRepository;

    public async Task<IResult<OffsetPageResponse<DataTransferObjectResponse>>> Handle(GenericOffsetPageQuery<TEntity, TEntityId> pageQuery, CancellationToken cancellationToken)
    {
        var page = await _proxyRepository
            .PageAsync(pageQuery.Page, cancellationToken, filter: pageQuery.Filter, sort: pageQuery.SortBy, mapping: pageQuery.Mapping);

        return page
            .ToPageResponse(pageQuery.Page)
            .ToResult();
    }
}
