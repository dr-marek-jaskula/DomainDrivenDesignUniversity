using Shopway.Application.Features.Products.Queries.DynamicOffsetProductWithMappingQuery;
using Shopway.Application.Features.Products.Queries.ProductCursorPageDynamicWithMappingQuery;
using Shopway.Application.Features.Proxy.PageQuery;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Products;
using Shopway.Domain.Products.DataProcessing.Filtering;
using Shopway.Domain.Products.DataProcessing.Mapping;
using Shopway.Domain.Products.DataProcessing.Sorting;

namespace Shopway.Application.Features.Proxy;

public partial class MediatorProxyService
{
    [PageQueryStrategy(nameof(Product), typeof(OffsetPage))]
    private static ProductOffsetPageDynamicWithMappingQuery QueryProductsUsingOffsetPage(ProxyPageQuery proxyQuery)
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

    [PageQueryStrategy(nameof(Product), typeof(CursorPage))]
    private static ProductCursorPageDynamicWithMappingQuery QueryProductsUsingCursorPage(ProxyPageQuery proxyQuery)
    {
        var page = proxyQuery.Page.ToCursorPage();
        var filter = proxyQuery.Filter?.To<ProductDynamicFilter, Product>();
        var sortBy = proxyQuery.SortBy?.To<ProductDynamicSortBy, Product>();
        var mapping = proxyQuery.Mapping?.To<ProductDynamicMapping, Product>();

        bool noMappingForCursorWhenMappingIsNotNull = mapping is not null && mapping
            .MappingEntries
            .Any(x => x.PropertyName is nameof(Product.Id)) is false;

        if (noMappingForCursorWhenMappingIsNotNull)
        {
            mapping!.MappingEntries.Insert(0, new MappingEntry()
            {
                PropertyName = nameof(Product.Id)
            });
        }

        return new ProductCursorPageDynamicWithMappingQuery(page)
        {
            Filter = filter,
            SortBy = sortBy,
            Mapping = mapping,
        };
    }
}
