using Shopway.Application.Abstractions.CQRS;
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
    private static readonly FrozenDictionary<PageQueryDiscriminator, Func<ProxyPageQuery, IQuery<PageResponse<DataTransferObjectResponse>>>> _strategyPageQueryCache =
        StrategyCacheFactory<PageQueryDiscriminator, Func<ProxyPageQuery, IQuery<PageResponse<DataTransferObjectResponse>>>>
            .CreateFor<MediatorProxyService, PageQueryStrategyAttribute>();

    private static readonly FrozenDictionary<QueryDiscriminator, Func<ProxyQuery, IQuery<DataTransferObjectResponse>>> _strategyQueryCache =
        StrategyCacheFactory<QueryDiscriminator, Func<ProxyQuery, IQuery<DataTransferObjectResponse>>>
            .CreateFor<MediatorProxyService, QueryStrategyAttribute>();

    private static readonly FrozenDictionary<GenericPageQueryDiscriminator, Func<GenericProxyPageQuery, IQuery<PageResponse<DataTransferObjectResponse>>>> _strategyGenericPageQueryCache =
        StrategyCacheFactory<GenericPageQueryDiscriminator, Func<GenericProxyPageQuery, IQuery<PageResponse<DataTransferObjectResponse>>>>
            .CreateFor<MediatorProxyService, GenericPageQueryStrategyAttribute>();

    private static readonly FrozenDictionary<GenericByIdQueryDiscriminator, Func<GenericProxyByIdQuery, IQuery<DataTransferObjectResponse>>> _strategyGenericQueryByIdCache =
        StrategyCacheFactory<GenericByIdQueryDiscriminator, Func<GenericProxyByIdQuery, IQuery<DataTransferObjectResponse>>>
            .CreateFor<MediatorProxyService, GenericByIdQueryStrategyAttribute>();

    private static readonly FrozenDictionary<GenericByKeyQueryDiscriminator, Func<GenericProxyByKeyQuery, IQuery<DataTransferObjectResponse>>> _strategyGenericQueryByKeyCache =
        StrategyCacheFactory<GenericByKeyQueryDiscriminator, Func<GenericProxyByKeyQuery, IQuery<DataTransferObjectResponse>>>
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
        if (proxyQuery.Page.PageIsNotOffsetOrCursorPage())
        {
            return Error.InvalidArgument("Cursor or PageNumber must be provided.")
                .ToResult<IQuery<PageResponse<DataTransferObjectResponse>>>();
        }

        if (proxyQuery.Page.PageIsBothOffsetAndCursorPage())
        {
            return Error.InvalidArgument("Both Cursor and PageNumber cannot be provided.")
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

    public Result<IQuery<DataTransferObjectResponse>> GenericMap(GenericProxyByIdQuery proxyQuery)
    {
        if (proxyQuery.Mapping is null || proxyQuery.Mapping.MappingEntries.IsEmpty())
        {
            return Error.InvalidArgument("Mapping must be provided.")
                .ToResult<IQuery<DataTransferObjectResponse>>();
        }

        var strategyKey = new GenericByIdQueryDiscriminator(proxyQuery.Entity);

        if (_strategyGenericQueryByIdCache.TryGetValue(strategyKey, out var @delegate) is false)
        {
            return Error.InvalidOperation($"Entity '{proxyQuery.Entity}' is not supported. Supported strategies: [{string.Join(", ", _strategyGenericQueryByIdCache.Keys.Select(x => x.Entity))}]")
                .ToResult<IQuery<DataTransferObjectResponse>>();
        }

        return Result.Success(@delegate!(proxyQuery));
    }

    public Result<IQuery<DataTransferObjectResponse>> GenericMap(GenericProxyByKeyQuery proxyQuery)
    {
        if (proxyQuery.Mapping is null || proxyQuery.Mapping.MappingEntries.IsEmpty())
        {
            return Error.InvalidArgument("Mapping must be provided.")
                .ToResult<IQuery<DataTransferObjectResponse>>();
        }

        var strategyKey = new GenericByKeyQueryDiscriminator(proxyQuery.Entity);

        if (_strategyGenericQueryByKeyCache.TryGetValue(strategyKey, out var @delegate) is false)
        {
            return Error.InvalidOperation($"Entity '{proxyQuery.Entity}' is not supported. Supported strategies: [{string.Join(", ", _strategyGenericQueryByKeyCache.Keys.Select(x => x.Entity))}]")
                .ToResult<IQuery<DataTransferObjectResponse>>();
        }

        return Result.Success(@delegate!(proxyQuery));
    }

    public Result<IQuery<PageResponse<DataTransferObjectResponse>>> GenericMap(GenericProxyPageQuery proxyQuery)
    {
        if (proxyQuery.Page.PageIsNotOffsetOrCursorPage())
        {
            return Error.InvalidArgument("Cursor or PageNumber must be provided.")
                .ToResult<IQuery<PageResponse<DataTransferObjectResponse>>>();
        }

        if (proxyQuery.Page.PageIsBothOffsetAndCursorPage())
        {
            return Error.InvalidArgument("Both Cursor and PageNumber cannot be provided.")
                .ToResult<IQuery<PageResponse<DataTransferObjectResponse>>>();
        }

        var strategyKey = new GenericPageQueryDiscriminator(proxyQuery.Entity, proxyQuery.Page.GetPageType());

        if (_strategyGenericPageQueryCache.TryGetValue(strategyKey, out var @delegate) is false)
        {
            return Error.InvalidOperation($"Entity '{proxyQuery.Entity}' with page type '{proxyQuery.Page.GetPageType().Name}' is not supported. Supported strategies: [{string.Join(", ", _strategyGenericPageQueryCache.Keys.Select(x => (x.Entity, x.PageType.Name)))}]")
                .ToResult<IQuery<PageResponse<DataTransferObjectResponse>>>();
        }

        return Result.Success(@delegate!(proxyQuery));
    }
}
