using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Features;
using Shopway.Application.Features.Proxy;
using Shopway.Presentation.Abstractions;

namespace Shopway.Presentation.Controllers;

[ApiVersion("2.0")]
public sealed class ProxyController(ISender sender, IMediatorProxyService genericMappingService) : ApiController(sender)
{
    private readonly IMediatorProxyService _genericMappingService = genericMappingService;

    [HttpPost("query")]
    [ProducesResponseType<PageResponse<DataTransferObjectResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<object>, ProblemHttpResult>> DynamicProxyQuery
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

        object concretePageQuery = queryResult.Value;

        var result = await Sender.Send(concretePageQuery, cancellationToken) as Shopway.Domain.Common.Results.IResult<object>;

        return result!.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }
}