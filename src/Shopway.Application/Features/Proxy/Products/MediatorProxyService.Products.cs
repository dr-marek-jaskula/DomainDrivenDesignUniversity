using Shopway.Application.Features.Products.Queries.DynamicOffsetProductWithMappingQuery;
using Shopway.Domain.Products;
using Shopway.Domain.Products.DataProcessing.Filtering;
using Shopway.Domain.Products.DataProcessing.Mapping;
using Shopway.Domain.Products.DataProcessing.Sorting;

namespace Shopway.Application.Features.Proxy;

public partial class MediatorProxyService
{
    [QueryStrategy("Product")]
    private static ProductOffsetPageDynamicWithMappingQuery QueryProducts(ProxyQuery proxyQuery)
    {
        var page = proxyQuery.Page.ToOffsetPage();
        var filter = proxyQuery.Filter?.To<ProductDynamicFilter, Product>();
        var sortBy = proxyQuery.SortBy?.To<ProductDynamicSortBy, Product>();
        var mapping = proxyQuery.Mapping?.To<ProductDynamicMapping, Product>();

        return new ProductOffsetPageDynamicWithMappingQuery(page)
        {
            Filter = filter,
            SortBy = sortBy,
            Mapping = mapping,
        };
    }
}