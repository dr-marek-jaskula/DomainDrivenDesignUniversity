using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Shopway.Domain.EntityIds;
using Microsoft.AspNetCore.Http;
using Shopway.Presentation.Abstractions;
using Shopway.Application.Features.Orders.Queries;
using Shopway.Application.Features.Orders.Queries.GetOrderById;
using Shopway.Application.Features.Orders.Commands.CreateHeaderOrder;
using Shopway.Application.Features.Orders.Commands.SoftDeleteOrderHeader;
using Shopway.Application.Features.Orders.Commands.ChangeOrderHeaderStatus;

namespace Shopway.Presentation.Controllers;

[ApiVersion("0.1", Deprecated = true)]
public sealed partial class OrderHeadersController(ISender sender) : ApiController(sender)
{
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderHeaderResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> GetOrderHeaderById([FromRoute] OrderHeaderId id, CancellationToken cancellationToken)
    {
        var query = new GetOrderHeaderByIdQuery(id);

        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateOrderHeaderResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> CreateOrderHeader
    (
        [FromBody] CreateOrderHeaderCommand command, 
        CancellationToken cancellationToken
    )
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return CreatedAtActionResult(result, nameof(GetOrderHeaderById));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> SoftDeleteOrderHeader
    (
        [FromRoute] OrderHeaderId id, 
        CancellationToken cancellationToken
    )
    {
        var command = new SoftDeleteOrderHeaderCommand(id);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok();
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ChangeOrderHeaderStatusResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> ChangeOrderHeaderStatus
    (
        [FromRoute] OrderHeaderId id,
        [FromBody] ChangeOrderHeaderStatusCommand.ChangeOrderHeaderStatusRequestBody body, 
        CancellationToken cancellationToken
    )
    {
        var command = new ChangeOrderHeaderStatusCommand(id, body);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }
}

