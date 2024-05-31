using Shopway.Application.Features.Proxy.GenericQuery.QueryByKey;
using Shopway.Domain.EntityKeys;
using Shopway.Domain.Products;

namespace Shopway.Application.Features.Proxy;

public partial class MediatorProxyService
{
    //Products
    [GenericByKeyQueryStrategy(nameof(Product))]
    private static GenericByKeyQuery<Product, ProductId, ProductKey> GenericQueryProductByKey(GenericProxyByKeyQuery proxyQuery)
        => GenericByKeyQuery<Product, ProductId, ProductKey>.From(proxyQuery);
}
