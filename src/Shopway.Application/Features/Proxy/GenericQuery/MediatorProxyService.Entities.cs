using Shopway.Application.Features.Proxy.GenericQuery.QueryById;
using Shopway.Domain.Orders;
using Shopway.Domain.Products;
using Shopway.Domain.Users;

namespace Shopway.Application.Features.Proxy;

public partial class MediatorProxyService
{
    //Order Headers
    [GenericQueryStrategy(nameof(OrderHeader))]
    private static GenericQuery<OrderHeader, OrderHeaderId> GenericQueryOrderHeaderById(GenericProxyQuery proxyQuery)
        => GenericQuery<OrderHeader, OrderHeaderId>.From(proxyQuery);

    //Products
    [GenericQueryStrategy(nameof(Product))]
    private static GenericQuery<Product, ProductId> GenericQueryProductById(GenericProxyQuery proxyQuery)
        => GenericQuery<Product, ProductId>.From(proxyQuery);

    //Users
    [GenericQueryStrategy(nameof(User))]
    private static GenericQuery<User, UserId> GenericQueryUserById(GenericProxyQuery proxyQuery)
        => GenericQuery<User, UserId>.From(proxyQuery);
}
