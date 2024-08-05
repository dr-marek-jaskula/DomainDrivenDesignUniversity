using Shopway.Application.Features.Proxy.GenericPageQuery;
using Shopway.Application.Features.Proxy.GenericQuery;
using Shopway.Application.Features.Proxy.GenericQuery.QueryById;
using Shopway.Application.Features.Proxy.GenericQuery.QueryByKey;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.EntityKeys;
using Shopway.Domain.Products;

namespace Shopway.Application.Features.Proxy;

public partial class MediatorProxyService
{
    [GenericPageQueryStrategy(nameof(Product), nameof(OffsetPage))]
    private static GenericOffsetPageQuery<Product, ProductId> GenericQueryProductsUsingOffsetPage(GenericProxyPageQuery proxyQuery)
        => GenericOffsetPageQuery<Product, ProductId>.From(proxyQuery);

    [GenericPageQueryStrategy(nameof(Product), nameof(CursorPage))]
    private static GenericCursorPageQuery<Product, ProductId> GenericQueryProductsUsingCursorPage(GenericProxyPageQuery proxyQuery)
        => GenericCursorPageQuery<Product, ProductId>.From(proxyQuery);

    [GenericByIdQueryStrategy(nameof(Product))]
    private static GenericByIdQuery<Product, ProductId> GenericQueryProductById(GenericProxyByIdQuery proxyQuery)
        => GenericByIdQuery<Product, ProductId>.From(proxyQuery);

    [GenericByKeyQueryStrategy(nameof(Product))]
    private static GenericByKeyQuery<Product, ProductId, ProductKey> GenericQueryProductByKey(GenericProxyByKeyQuery proxyQuery)
        => GenericByKeyQuery<Product, ProductId, ProductKey>.From(proxyQuery);
}
