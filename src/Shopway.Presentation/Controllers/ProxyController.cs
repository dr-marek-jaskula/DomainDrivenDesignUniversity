using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Features;
using Shopway.Application.Features.Proxy;
using Shopway.Application.Features.Proxy.GenericQuery.QueryByKey;
using Shopway.Application.Features.Proxy.PageQuery;
using Shopway.Application.Features.Proxy.Query;
using Shopway.Presentation.Abstractions;
using Shopway.Presentation.Utilities;

namespace Shopway.Presentation.Controllers;

[ApiVersion("2.0")]
public sealed class ProxyController(ISender sender, IMediatorProxyService genericMappingService) : ApiController(sender)
{
    private readonly IMediatorProxyService _genericMappingService = genericMappingService;

    [HttpPost("query")]
    [ProducesResponseType<DataTransferObjectResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<DataTransferObjectResponse>, ProblemHttpResult>> DynamicProxyQuery
    (
        [FromBody] ProxyQuery query,
        CancellationToken cancellationToken
    )
    {
        var queryResult = _genericMappingService.Map(query);

        if (queryResult!.IsFailure)
        {
            return queryResult.ToProblemHttpResult();
        }

        var result = await Sender.Send(queryResult.Value, cancellationToken);

        return result!.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    [HttpPost("query/page")]
    [ProducesResponseType<PageResponse<DataTransferObjectResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<object>, ProblemHttpResult>> DynamicProxyPageQuery
    (
        [FromBody] ProxyPageQuery query,
        CancellationToken cancellationToken
    )
    {
        var queryResult = _genericMappingService.Map(query);

        if (queryResult!.IsFailure)
        {
            return queryResult.ToProblemHttpResult();
        }

        object concretePageQuery = queryResult.Value;

        var result = await Sender.Send(concretePageQuery, cancellationToken) as Shopway.Domain.Common.Results.IResult<object>;

        return result!.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    [HttpPost("query/id/generic")]
    [ProducesResponseType<DataTransferObjectResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<DataTransferObjectResponse>, ProblemHttpResult>> GenericProxyQueryById
    (
        [FromBody] GenericProxyByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        var queryResult = _genericMappingService.GenericMap(query);

        if (queryResult!.IsFailure)
        {
            return queryResult.ToProblemHttpResult();
        }

        var result = await Sender.Send(queryResult.Value, cancellationToken);

        return result!.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    [HttpPost("query/key/generic")]
    [ProducesResponseType<DataTransferObjectResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<DataTransferObjectResponse>, ProblemHttpResult>> GenericProxyQueryByKey
    (
        [FromBody] GenericProxyByKeyQuery query,
        CancellationToken cancellationToken
    )
    {
        var queryResult = _genericMappingService.GenericMap(query);

        if (queryResult!.IsFailure)
        {
            return queryResult.ToProblemHttpResult();
        }

        var result = await Sender.Send(queryResult.Value, cancellationToken);

        return result!.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    [HttpPost("query/page/generic")]
    [ProducesResponseType<PageResponse<DataTransferObjectResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<object>, ProblemHttpResult>> GenericProxyPageQuery
    (
        [FromBody] GenericProxyPageQuery query,
        CancellationToken cancellationToken
    )
    {
        var queryResult = _genericMappingService.GenericMap(query);

        if (queryResult!.IsFailure)
        {
            return queryResult.ToProblemHttpResult();
        }

        object concretePageQuery = queryResult.Value;

        var result = await Sender.Send(concretePageQuery, cancellationToken) as Shopway.Domain.Common.Results.IResult<object>;

        return result!.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }
}
