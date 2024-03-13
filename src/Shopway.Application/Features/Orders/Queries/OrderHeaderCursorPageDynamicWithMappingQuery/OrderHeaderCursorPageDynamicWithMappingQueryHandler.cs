using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Orders.DataProcessing.Filtering;
using Shopway.Domain.Orders.DataProcessing.Mapping;
using Shopway.Domain.Orders.DataProcessing.Sorting;
using Shopway.Domain.Orders;
using Shopway.Application.Utilities;

namespace Shopway.Application.Features.Orders.Queries.DynamicCursorOrderHeaderWithMappingQuery;

internal sealed class OrderHeaderCursorPageDynamicWithMappingQueryHandler(IOrderHeaderRepository orderHeaderRepository)
    : ICursorPageQueryHandler<OrderHeaderCursorPageDynamicWithMappingQuery, DataTransferObjectResponse, OrderHeaderDynamicFilter, OrderHeaderDynamicSortBy, OrderHeaderDynamicMapping, CursorPage>
{
    private readonly IOrderHeaderRepository _orderHeaderRepository = orderHeaderRepository;

    public async Task<IResult<CursorPageResponse<DataTransferObjectResponse>>> Handle(OrderHeaderCursorPageDynamicWithMappingQuery pageQuery, CancellationToken cancellationToken)
    {
        var page = await _orderHeaderRepository
            .PageAsync(pageQuery.Page, cancellationToken, filter: pageQuery.Filter, sort: pageQuery.SortBy, mapping: pageQuery.Mapping);

        return page
            .ToPageResponse(pageQuery.Page)
            .ToResult();
    }
}
