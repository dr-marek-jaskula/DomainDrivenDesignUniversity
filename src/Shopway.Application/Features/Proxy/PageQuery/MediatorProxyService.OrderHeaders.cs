using Shopway.Application.Features.Orders.Queries.DynamicCursorOrderHeaderWithMappingQuery;
using Shopway.Application.Features.Orders.Queries.DynamicOffsetOrderHeaderWithMappingQuery;
using Shopway.Application.Features.Proxy.PageQuery;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Orders;
using Shopway.Domain.Orders.DataProcessing.Filtering;
using Shopway.Domain.Orders.DataProcessing.Mapping;
using Shopway.Domain.Orders.DataProcessing.Sorting;

namespace Shopway.Application.Features.Proxy;

public partial class MediatorProxyService
{
    [PageQueryStrategy(nameof(OrderHeader), typeof(OffsetPage))]
    private static OrderHeaderOffsetPageDynamicWithMappingQuery QueryOrderHeadersUsingOffsetPage(ProxyPageQuery proxyQuery)
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

    [PageQueryStrategy(nameof(OrderHeader), typeof(CursorPage))]
    private static OrderHeaderCursorPageDynamicWithMappingQuery QueryOrderHeadersUsingCursorPage(ProxyPageQuery proxyQuery)
    {
        var page = proxyQuery.Page.ToCursorPage();
        var filter = proxyQuery.Filter?.To<OrderHeaderDynamicFilter, OrderHeader>();
        var sortBy = proxyQuery.SortBy?.To<OrderHeaderDynamicSortBy, OrderHeader>();
        var mapping = proxyQuery.Mapping?.To<OrderHeaderDynamicMapping, OrderHeader>();

        bool noMappingForCursorWhenMappingIsNotNull = mapping is not null && mapping
            .MappingEntries
            .Any(x => x.PropertyName is nameof(OrderHeader.Id)) is false;

        if (noMappingForCursorWhenMappingIsNotNull)
        {
            mapping!.MappingEntries.Insert(0, new MappingEntry()
            {
                PropertyName = nameof(OrderHeader.Id)
            });
        }

        return new OrderHeaderCursorPageDynamicWithMappingQuery(page)
        {
            Filter = filter,
            SortBy = sortBy,
            Mapping = mapping,
        };
    }
}
