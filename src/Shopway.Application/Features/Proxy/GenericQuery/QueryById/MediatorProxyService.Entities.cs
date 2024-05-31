using Shopway.Application.Features.Proxy.GenericQuery.QueryById;
using Shopway.Domain.Orders;
using Shopway.Domain.Products;
using Shopway.Domain.Users;

namespace Shopway.Application.Features.Proxy;

public partial class MediatorProxyService
{
    //Order Headers
    [GenericByIdQueryStrategy(nameof(OrderHeader))]
    private static GenericByIdQuery<OrderHeader, OrderHeaderId> GenericQueryOrderHeaderById(GenericProxyByIdQuery proxyQuery)
        => GenericByIdQuery<OrderHeader, OrderHeaderId>.From(proxyQuery);

    //Products
    [GenericByIdQueryStrategy(nameof(Product))]
    private static GenericByIdQuery<Product, ProductId> GenericQueryProductById(GenericProxyByIdQuery proxyQuery)
        => GenericByIdQuery<Product, ProductId>.From(proxyQuery);

    //Users
    [GenericByIdQueryStrategy(nameof(User))]
    private static GenericByIdQuery<User, UserId> GenericQueryUserById(GenericProxyByIdQuery proxyQuery)
        => GenericByIdQuery<User, UserId>.From(proxyQuery);
}
