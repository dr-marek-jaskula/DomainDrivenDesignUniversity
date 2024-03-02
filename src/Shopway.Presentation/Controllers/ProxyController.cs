using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Features;
using Shopway.Domain.Common.Results;
using Shopway.Presentation.Abstractions;

namespace Shopway.Presentation.Controllers;

[ApiVersion("2.0")]
public sealed class ProxyController(ISender sender, IMediatorProxyService genericMappingService) : ApiController(sender)
{
    private readonly IMediatorProxyService _genericMappingService = genericMappingService;

    [HttpPost("query")]
    [ProducesResponseType<PageResponse<DataTransferObjectResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> QueryProductsCursorDictionary
    (
        [FromBody] ProxyQuery query,
        CancellationToken cancellationToken
    )
    {
        var queryResult = _genericMappingService.Map(query);

        if (queryResult!.IsFailure)
        {
            return HandleFailure(queryResult);
        }

        object concretePageQuery = queryResult.Value;

        var result = await Sender.Send(concretePageQuery, cancellationToken) as IResult<object>;

        if (result!.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }
}