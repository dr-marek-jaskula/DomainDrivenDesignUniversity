using Shopway.Application.Abstractions;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Orders.DataProcessing.Filtering;
using Shopway.Domain.Orders.DataProcessing.Mapping;
using Shopway.Domain.Orders.DataProcessing.Sorting;

namespace Shopway.Application.Features.Orders.Queries.DynamicCursorOrderHeaderWithMappingQuery;

internal sealed class OrderHeaderCursorPageDynamicWithMappingQueryValidator : CursorPageQueryValidator<OrderHeaderCursorPageDynamicWithMappingQuery, DataTransferObjectResponse, OrderHeaderDynamicFilter, OrderHeaderDynamicSortBy, OrderHeaderDynamicMapping, CursorPage>
{
    public OrderHeaderCursorPageDynamicWithMappingQueryValidator()
        : base()
    {
    }
}
