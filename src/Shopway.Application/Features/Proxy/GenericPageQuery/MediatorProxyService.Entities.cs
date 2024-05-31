using Shopway.Application.Features.Proxy.GenericPageQuery;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Orders;
using Shopway.Domain.Products;
using Shopway.Domain.Users;

namespace Shopway.Application.Features.Proxy;

public partial class MediatorProxyService
{
    //Order Headers
    [GenericPageQueryStrategy(nameof(OrderHeader), typeof(OffsetPage))]
    private static GenericOffsetPageQuery<OrderHeader, OrderHeaderId> GenericQueryOrderHeadersUsingOffsetPage(GenericProxyPageQuery proxyQuery)
        => GenericOffsetPageQuery<OrderHeader, OrderHeaderId>.From(proxyQuery);

    [GenericPageQueryStrategy(nameof(OrderHeader), typeof(CursorPage))]
    private static GenericCursorPageQuery<OrderHeader, OrderHeaderId> GenericQueryOrderHeadersUsingCursorPage(GenericProxyPageQuery proxyQuery)
        => GenericCursorPageQuery<OrderHeader, OrderHeaderId>.From(proxyQuery);

    //Products
    [GenericPageQueryStrategy(nameof(Product), typeof(OffsetPage))]
    private static GenericOffsetPageQuery<Product, ProductId> GenericQueryProductsUsingOffsetPage(GenericProxyPageQuery proxyQuery)
        => GenericOffsetPageQuery<Product, ProductId>.From(proxyQuery);

    [GenericPageQueryStrategy(nameof(Product), typeof(CursorPage))]
    private static GenericCursorPageQuery<Product, ProductId> GenericQueryProductsUsingCursorPage(GenericProxyPageQuery proxyQuery)
        => GenericCursorPageQuery<Product, ProductId>.From(proxyQuery);

    //Users
    [GenericPageQueryStrategy(nameof(User), typeof(OffsetPage))]
    private static GenericOffsetPageQuery<User, UserId> GenericQueryUsersUsingOffsetPage(GenericProxyPageQuery proxyQuery)
        => GenericOffsetPageQuery<User, UserId>.From(proxyQuery);

    [GenericPageQueryStrategy(nameof(User), typeof(CursorPage))]
    private static GenericCursorPageQuery<User, UserId> GenericQueryUsersUsingCursorPage(GenericProxyPageQuery proxyQuery)
        => GenericCursorPageQuery<User, UserId>.From(proxyQuery);
}
