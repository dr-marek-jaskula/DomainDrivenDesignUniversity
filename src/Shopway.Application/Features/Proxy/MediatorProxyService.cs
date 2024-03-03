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
        _validator
            .If(PageIsNotOffsetOrCursorPage(genericPageQuery.Page), Error.InvalidArgument("Cursor or PageNumber must be provided."))
            .If(PageIsBothOffsetAndCursorPage(genericPageQuery.Page), Error.InvalidArgument("Both Cursor and PageNumber cannot be provided."));

        if (_validator.IsInvalid)
        {
            return Failure();
        }

        var strategyKey = new QueryDiscriminator(genericPageQuery.Entity, genericPageQuery.Page.GetPageType());

        _validator
            .If(_strategyCache.TryGetValue(strategyKey, out var @delegate) is false, Error.InvalidOperation($"Entity '{genericPageQuery.Entity}' with page type '{genericPageQuery.Page.GetPageType().Name}' is not supported. Supported strategies: [{string.Join(", ", _strategyCache.Keys.Select(x => (x.Entity, x.PageType.Name)))}]"));

        if (_validator.IsInvalid)
        {
            return Failure();
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

    private ValidationResult<IQuery<PageResponse<DataTransferObjectResponse>>> Failure()
    {
        return ValidationResult<IQuery<PageResponse<DataTransferObjectResponse>>>
            .WithErrors(_validator.Failure().ValidationErrors);
    }
}