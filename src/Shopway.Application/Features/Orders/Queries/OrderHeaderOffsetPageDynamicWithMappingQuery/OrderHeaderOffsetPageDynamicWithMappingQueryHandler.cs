using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Orders;
using Shopway.Domain.Orders.DataProcessing.Filtering;
using Shopway.Domain.Orders.DataProcessing.Mapping;
using Shopway.Domain.Orders.DataProcessing.Sorting;

namespace Shopway.Application.Features.Orders.Queries.DynamicOffsetOrderHeaderWithMappingQuery;

internal sealed class OrderHeaderOffsetPageDynamicWithMappingQueryHandler(IOrderHeaderRepository orderHeaderRepository)
    : IOffsetPageQueryHandler<OrderHeaderOffsetPageDynamicWithMappingQuery, DataTransferObjectResponse, OrderHeaderDynamicFilter, OrderHeaderDynamicSortBy, OrderHeaderDynamicMapping, OffsetPage>
{
    private readonly IOrderHeaderRepository _orderHeaderRepository = orderHeaderRepository;

    public async Task<IResult<OffsetPageResponse<DataTransferObjectResponse>>> Handle(OrderHeaderOffsetPageDynamicWithMappingQuery pageQuery, CancellationToken cancellationToken)
    {
        var page = await _orderHeaderRepository
            .PageAsync(pageQuery.Page, cancellationToken, filter: pageQuery.Filter, sort: pageQuery.SortBy, mapping: pageQuery.Mapping);

        return page
            .ToPageResponse(pageQuery.Page)
            .ToResult();
    }
}
