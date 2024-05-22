using Shopway.Application.Features.Products.Queries.DynamicProductWithMappingQuery;
using Shopway.Application.Features.Proxy.Query;
using Shopway.Domain.Products;
using Shopway.Domain.Products.DataProcessing.Mapping;

namespace Shopway.Application.Features.Proxy;

public partial class MediatorProxyService
{
    [QueryStrategy(nameof(Product))]
    private static ProductWithMappingQuery QueryProductById(ProxyQuery proxyQuery)
    {
        var mapping = proxyQuery.Mapping?.To<ProductDynamicMapping, Product>();

        return new ProductWithMappingQuery(ProductId.Create(proxyQuery.Id))
        {
            Mapping = mapping,
        };
    }
}
