using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.DataProcessing.Proxy;
using Shopway.Domain.Common.Disciminators;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Errors;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Shopway.Application.Features.Proxy;

public partial class MediatorProxyService(IValidator validator) : IMediatorProxyService
{
    private static readonly ReadOnlyDictionary<QueryDiscriminator, Func<ProxyQuery, IQuery<PageResponse<DataTransferObjectResponse>>>> _strategyCache;
    
    private readonly IValidator _validator = validator;

    static MediatorProxyService()
    {
        var strategies = typeof(MediatorProxyService)
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
            .Where(method => method.GetCustomAttribute<QueryStrategyAttribute>() is not null)
            .Select(x => x.CreateDelegate<Func<ProxyQuery, IQuery<PageResponse<DataTransferObjectResponse>>>>());

        _strategyCache = DiscriminatorCacheFactory<QueryDiscriminator, Func<ProxyQuery, IQuery<PageResponse<DataTransferObjectResponse>>>>
            .CreateFor<QueryStrategyAttribute>(strategies);
    }

    public Result<IQuery<PageResponse<DataTransferObjectResponse>>> Map(ProxyQuery genericPageQuery)
    {
        var strategyKey = new QueryDiscriminator(genericPageQuery.Entity);

        _validator
            .If(PageIsNotOffsetOrCursorPage(genericPageQuery.Page), Error.InvalidOperation("Cursor or PageNumber must be provided."))
            .If(PageIsBothOffsetAndCursorPage(genericPageQuery.Page), Error.InvalidOperation("Both Cursor and PageNumber cannot be provided."))
            .If(_strategyCache.TryGetValue(strategyKey, out var @delegate) is false, Error.InvalidOperation($"Entity '{genericPageQuery.Entity}' is not supported. Supported entities: [{string.Join(", ", _strategyCache.Keys.Select(x => x.Entity))}]"));

        if (_validator.IsInvalid)
        {
            return ValidationResult<IQuery<PageResponse<DataTransferObjectResponse>>>
                .WithErrors(_validator.Failure().ValidationErrors);
        }

        return Result.Success(@delegate!(genericPageQuery));
    }

    private static bool PageIsNotOffsetOrCursorPage(OffsetOrCursorPage offsetOrCursorPage)
    {
        return offsetOrCursorPage.Cursor is null && offsetOrCursorPage.PageNumber is null;
    }

    private static bool PageIsBothOffsetAndCursorPage(OffsetOrCursorPage offsetOrCursorPage)
    {
        return offsetOrCursorPage.Cursor is not null && offsetOrCursorPage.PageNumber is not null;
    }
}