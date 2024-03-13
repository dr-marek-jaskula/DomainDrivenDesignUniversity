using Shopway.Application.Features.Orders.Queries.DynamicOffsetOrderHeaderWithMappingQuery;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Orders;
using Shopway.Domain.Orders.DataProcessing.Filtering;
using Shopway.Domain.Orders.DataProcessing.Mapping;
using Shopway.Domain.Orders.DataProcessing.Sorting;

namespace Shopway.Application.Features.Proxy;

public partial class MediatorProxyService
{
    [QueryStrategy(nameof(OrderHeader), typeof(OffsetPage))]
    private static OrderHeaderOffsetPageDynamicWithMappingQuery QueryOrderHeadersUsingOffsetPage(ProxyQuery proxyQuery)
    {
        var page = proxyQuery.Page.ToOffsetPage();
        var filter = proxyQuery.Filter?.To<OrderHeaderDynamicFilter, OrderHeader>();
        var sortBy = proxyQuery.SortBy?.To<OrderHeaderDynamicSortBy, OrderHeader>();
        var mapping = proxyQuery.Mapping?.To<OrderHeaderDynamicMapping, OrderHeader>();

        return new OrderHeaderOffsetPageDynamicWithMappingQuery(page)
        {
            Filter = filter,
            SortBy = sortBy,
            Mapping = mapping,
        };
    }
}