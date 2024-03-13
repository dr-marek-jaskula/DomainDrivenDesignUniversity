using Shopway.Application.Abstractions;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Orders.DataProcessing.Filtering;
using Shopway.Domain.Orders.DataProcessing.Mapping;
using Shopway.Domain.Orders.DataProcessing.Sorting;

namespace Shopway.Application.Features.Orders.Queries.DynamicOffsetOrderHeaderWithMappingQuery;

internal sealed class OrderHeaderOffsetPageDynamicWithMappingQueryValidator : OffsetPageQueryValidator<OrderHeaderOffsetPageDynamicWithMappingQuery, DataTransferObjectResponse, OrderHeaderDynamicFilter, OrderHeaderDynamicSortBy, OrderHeaderDynamicMapping, OffsetPage>
{
    public OrderHeaderOffsetPageDynamicWithMappingQueryValidator()
        : base()
    {
    }
}