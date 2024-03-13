using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Orders.DataProcessing.Filtering;
using Shopway.Domain.Orders.DataProcessing.Mapping;
using Shopway.Domain.Orders.DataProcessing.Sorting;

namespace Shopway.Application.Features.Orders.Queries.DynamicOffsetOrderHeaderWithMappingQuery;

public sealed record OrderHeaderOffsetPageDynamicWithMappingQuery(OffsetPage Page) : IOffsetPageQuery<DataTransferObjectResponse, OrderHeaderDynamicFilter, OrderHeaderDynamicSortBy, OrderHeaderDynamicMapping, OffsetPage>
{
    public OrderHeaderDynamicFilter? Filter { get; init; }
    public OrderHeaderDynamicSortBy? SortBy { get; init; }
    public OrderHeaderDynamicMapping? Mapping { get; init; }
}