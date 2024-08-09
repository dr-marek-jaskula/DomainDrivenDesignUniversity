using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Features.Proxy.GenericQuery;
using Shopway.Application.Features.Proxy.GenericQuery.QueryById;
using Shopway.Application.Features.Proxy.GenericQuery.QueryByKey;
using Shopway.Application.Features.Proxy.PageQuery;
using Shopway.Application.Features.Proxy.Query;
using Shopway.Domain.Common.Discriminators;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using System.Collections.Frozen;

namespace Shopway.Application.Features.Proxy;

public partial class MediatorProxyService : IMediatorProxyService
{
    //Not generic caches
    private static readonly FrozenDictionary<PageQueryDiscriminator, Func<ProxyPageQuery, IQuery<PageResponse<DataTransferObjectResponse>>>> _strategyPageQueryCache =
        StrategyCacheFactory<PageQueryDiscriminator, Func<ProxyPageQuery, IQuery<PageResponse<DataTransferObjectResponse>>>>
            .CreateFor<MediatorProxyService, PageQueryStrategyAttribute>();

    private static readonly FrozenDictionary<QueryDiscriminator, Func<ProxyQuery, IQuery<DataTransferObjectResponse>>> _strategyQueryCache =
        StrategyCacheFactory<QueryDiscriminator, Func<ProxyQuery, IQuery<DataTransferObjectResponse>>>
            .CreateFor<MediatorProxyService, QueryStrategyAttribute>();

    //Generic caches
    private static readonly FrozenDictionary<string, Func<GenericProxyPageQuery, IQuery<PageResponse<DataTransferObjectResponse>>>> _strategyGenericPageQueryCache =
        StrategyCacheFactory<Func<GenericProxyPageQuery, IQuery<PageResponse<DataTransferObjectResponse>>>>
            .CreateFor<MediatorProxyService, GenericPageQueryStrategyAttribute>();

    private static readonly FrozenDictionary<string, Func<GenericProxyByIdQuery, IQuery<DataTransferObjectResponse>>> _strategyGenericQueryByIdCache =
        StrategyCacheFactory<Func<GenericProxyByIdQuery, IQuery<DataTransferObjectResponse>>>
            .CreateFor<MediatorProxyService, GenericByIdQueryStrategyAttribute>();

    private static readonly FrozenDictionary<string, Func<GenericProxyByKeyQuery, IQuery<DataTransferObjectResponse>>> _strategyGenericQueryByKeyCache =
        StrategyCacheFactory<Func<GenericProxyByKeyQuery, IQuery<DataTransferObjectResponse>>>
            .CreateFor<MediatorProxyService, GenericByKeyQueryStrategyAttribute>();

    public Result<IQuery<DataTransferObjectResponse>> Map(ProxyQuery proxyQuery)
    {
        if (proxyQuery.Mapping is null || proxyQuery.Mapping.MappingEntries.IsEmpty())
        {
            return Error.InvalidArgument("Mapping must be provided.")
                .ToResult<IQuery<DataTransferObjectResponse>>();
        }

        var strategyKey = new QueryDiscriminator(proxyQuery.Entity);

        if (_strategyQueryCache.TryGetValue(strategyKey, out var @delegate) is false)
        {
            return Error.InvalidOperation($"Entity '{proxyQuery.Entity}' is not supported. Supported strategies: [{string.Join(", ", _strategyQueryCache.Keys.Select(x => x.Entity))}]")
                .ToResult<IQuery<DataTransferObjectResponse>>();
        }

        return Result.Success(@delegate!(proxyQuery));
    }

    public Result<IQuery<PageResponse<DataTransferObjectResponse>>> Map(ProxyPageQuery proxyQuery)
    {
        var pageIsNotOffsetOrCursorPageResult = proxyQuery.Page.IsNotOffsetOrCursorPage();

        if (pageIsNotOffsetOrCursorPageResult.IsFailure)
        {
            return pageIsNotOffsetOrCursorPageResult.Error
                .ToResult<IQuery<PageResponse<DataTransferObjectResponse>>>();
        }

        var pageIsBothOffsetAndCursorPageResult = proxyQuery.Page.IsBothOffsetAndCursorPage();

        if (pageIsBothOffsetAndCursorPageResult.IsFailure)
        {
            return pageIsBothOffsetAndCursorPageResult.Error
                .ToResult<IQuery<PageResponse<DataTransferObjectResponse>>>();
        }

        var strategyKey = new PageQueryDiscriminator(proxyQuery.Entity, proxyQuery.Page.GetPageType());

        if (_strategyPageQueryCache.TryGetValue(strategyKey, out var @delegate) is false)
        {
            return Error.InvalidOperation($"Entity '{proxyQuery.Entity}' with page type '{proxyQuery.Page.GetPageType().Name}' is not supported. Supported strategies: [{string.Join(", ", _strategyPageQueryCache.Keys.Select(x => (x.Entity, x.PageType.Name)))}]")
                .ToResult<IQuery<PageResponse<DataTransferObjectResponse>>>();
        }

        return Result.Success(@delegate!(proxyQuery));
    }

    public Result<IQuery<DataTransferObjectResponse>> GenericMap(IProxyQueryWithMapping proxyQuery)
    {
        if (proxyQuery.Mapping is null || proxyQuery.Mapping.MappingEntries.IsEmpty())
        {
            return Error.InvalidArgument("Mapping must be provided.")
                .ToResult<IQuery<DataTransferObjectResponse>>();
        }

        return proxyQuery switch
        {
            GenericProxyByIdQuery queryById => GetResult(queryById, _strategyGenericQueryByIdCache),
            GenericProxyByKeyQuery queryByKey => GetResult(queryByKey, _strategyGenericQueryByKeyCache),
            _ => Error.InvalidArgument($"Not supported query type: '{proxyQuery.GetType()}'")
                    .ToResult<IQuery<DataTransferObjectResponse>>()
        };
    }

    public Result<IQuery<PageResponse<DataTransferObjectResponse>>> GenericMap(GenericProxyPageQuery proxyQuery)
    {
        var pageNameResult = proxyQuery.Page.GetPageName();

        if (pageNameResult.IsFailure)
        {
            return pageNameResult.Error.ToResult<IQuery<PageResponse<DataTransferObjectResponse>>>();
        }

        var key = GenericProxyPageQuery.GetCacheKey(pageNameResult.Value, proxyQuery.Entity);

        if (_strategyGenericPageQueryCache.TryGetValue(key, out var @delegate) is false)
        {
            return Error.InvalidOperation($"Entity '{proxyQuery.Entity}' with page type '{proxyQuery.Page.GetPageType().Name}' is not supported. Supported strategies: [{string.Join(", ", _strategyGenericPageQueryCache.Keys)}]")
                .ToResult<IQuery<PageResponse<DataTransferObjectResponse>>>();
        }

        return Result.Success(@delegate!(proxyQuery));
    }

    private static Result<IQuery<DataTransferObjectResponse>> GetResult<TQuery>(TQuery query, FrozenDictionary<string, Func<TQuery, IQuery<DataTransferObjectResponse>>> cache)
        where TQuery : IProxyQueryWithMapping
    {
        var result = cache.TryGetValue(query.GetCacheKey(), out var @delegate);

        if (result is false)
        {
            return Error.InvalidOperation($"Entity '{query.Entity}' is not supported. Supported strategies: [{string.Join(", ", cache.Keys.Select(GetSupportedEntities))}]")
                .ToResult<IQuery<DataTransferObjectResponse>>();
        }

        return Result.Success(@delegate!(query));
    }

    private static string GetSupportedEntities(string input)
    {
        return input.Split('-').Last();
    }
}
