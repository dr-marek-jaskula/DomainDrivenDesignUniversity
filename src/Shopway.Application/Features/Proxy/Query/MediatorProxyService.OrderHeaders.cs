using Shopway.Application.Features.Orders.Queries;
using Shopway.Application.Features.Proxy.Query;
using Shopway.Domain.Orders;
using Shopway.Domain.Orders.DataProcessing.Mapping;

namespace Shopway.Application.Features.Proxy;

public partial class MediatorProxyService
{
    [QueryStrategy(nameof(OrderHeader))]
    private static OrderHeaderWithMappingQuery QueryOrderHeaderById(ProxyQuery proxyQuery)
    {
        var mapping = proxyQuery.Mapping?.To<OrderHeaderDynamicMapping, OrderHeader>();

        return new OrderHeaderWithMappingQuery(OrderHeaderId.Create(proxyQuery.Id))
        {
            Mapping = mapping,
        };
    }
}
