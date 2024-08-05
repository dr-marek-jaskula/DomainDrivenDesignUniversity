using Shopway.Application.Features.Proxy.GenericPageQuery;
using Shopway.Application.Features.Proxy.GenericQuery;
using Shopway.Application.Features.Proxy.GenericQuery.QueryById;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Orders;

namespace Shopway.Application.Features.Proxy;

public partial class MediatorProxyService
{
    [GenericPageQueryStrategy(nameof(OrderHeader), nameof(OffsetPage))]
    private static GenericOffsetPageQuery<OrderHeader, OrderHeaderId> GenericQueryOrderHeadersUsingOffsetPage(GenericProxyPageQuery proxyQuery)
        => GenericOffsetPageQuery<OrderHeader, OrderHeaderId>.From(proxyQuery);

    [GenericPageQueryStrategy(nameof(OrderHeader), nameof(CursorPage))]
    private static GenericCursorPageQuery<OrderHeader, OrderHeaderId> GenericQueryOrderHeadersUsingCursorPage(GenericProxyPageQuery proxyQuery)
        => GenericCursorPageQuery<OrderHeader, OrderHeaderId>.From(proxyQuery);

    [GenericByIdQueryStrategy(nameof(OrderHeader))]
    private static GenericByIdQuery<OrderHeader, OrderHeaderId> GenericQueryOrderHeaderById(GenericProxyByIdQuery proxyQuery)
        => GenericByIdQuery<OrderHeader, OrderHeaderId>.From(proxyQuery);
}
