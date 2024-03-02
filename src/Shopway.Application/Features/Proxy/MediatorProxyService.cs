using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.Disciminators;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Shopway.Application.Features.Proxy;

public partial class MediatorProxyService : IMediatorProxyService
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
}