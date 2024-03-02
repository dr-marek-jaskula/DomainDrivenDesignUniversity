using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Features;
using Shopway.Application.Features.Products.Queries.DynamicOffsetProductWithMappingQuery;
using Shopway.Domain.Common.Disciminators;
using Shopway.Domain.Products;
using Shopway.Domain.Products.DataProcessing.Filtering;
using Shopway.Domain.Products.DataProcessing.Mapping;
using Shopway.Domain.Products.DataProcessing.Sorting;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Shopway.Infrastructure.Services.Proxy;

public sealed class MediatorProxyService : IMediatorProxyService
{
    private static readonly ReadOnlyDictionary<QueryDiscriminator, Func<ProxyQuery, IQuery<PageResponse<DataTransferObjectResponse>>>> _strategyCache;

    static MediatorProxyService()
    {
        var strategies = typeof(MediatorProxyService)
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
            .Where(method => method.GetCustomAttribute<QueryStrategyAttribute>() is not null)
            .Select(x => x.CreateDelegate<Func<ProxyQuery, IQuery<PageResponse<DataTransferObjectResponse>>>>());

        _strategyCache = DiscriminatorCacheFactory<QueryDiscriminator, Func<ProxyQuery, IQuery<PageResponse<DataTransferObjectResponse>>>>
            .CreateFor<QueryStrategyAttribute>(strategies);
    }

    public IQuery<PageResponse<DataTransferObjectResponse>> Map(ProxyQuery genericPageQuery)
    {
        var strategyKey = new QueryDiscriminator(genericPageQuery.Entity);

        if (_strategyCache.TryGetValue(strategyKey, out var @delegate) is false)
        {
            throw new ArgumentException($"Strategy for source type '{genericPageQuery.Entity}' is not supported.");
        }

        return @delegate(genericPageQuery);
    }

    [QueryStrategy("Product")]
    private static ProductOffsetPageDynamicWithMappingQuery QueryProducts(ProxyQuery genericPageQuery)
    {
        var page = genericPageQuery.Page.ToOffsetPage();
        var filter = genericPageQuery.Filter?.To<ProductDynamicFilter, Product>();
        var sortBy = genericPageQuery.SortBy?.To<ProductDynamicSortBy, Product>();
        var mapping = genericPageQuery.Mapping?.To<ProductDynamicMapping, Product>();

        return new ProductOffsetPageDynamicWithMappingQuery(page)
        {
            Filter = filter,
            SortBy = sortBy,
            Mapping = mapping,
        };
    }

    [QueryStrategy("OrderHeader")]
    private static IQuery<OffsetPageResponse<DataTransferObjectResponse>> QueryOrderHeaders(ProxyQuery genericPageQuery)
    {
        throw new NotImplementedException();
    }
}