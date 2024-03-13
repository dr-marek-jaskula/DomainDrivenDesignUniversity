using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shopway.Application.Features.Orders.Commands.ChangeOrderHeaderStatus;
using Shopway.Application.Features.Orders.Commands.CreateHeaderOrder;
using Shopway.Application.Features.Orders.Commands.SoftDeleteOrderHeader;
using Shopway.Application.Features.Orders.Queries;
using Shopway.Application.Features.Orders.Queries.GetOrderById;
using Shopway.Domain.Orders;
using Shopway.Presentation.Abstractions;

namespace Shopway.Presentation.Controllers;

[ApiVersion("0.1", Deprecated = true)]
public sealed partial class OrderHeadersController(ISender sender) : ApiController(sender)
{
    [HttpGet("{id}")]
    [ProducesResponseType<OrderHeaderResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<OrderHeaderResponse>, ProblemHttpResult>> GetOrderHeaderById
    (
        [FromRoute] OrderHeaderId id, 
        CancellationToken cancellationToken
    )
    {
        var query = new GetOrderHeaderByIdQuery(id);

        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return TypedResults.Ok(result.Value);
    }

    [HttpPost]
    [ProducesResponseType<CreateOrderHeaderResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<CreatedAtRoute<CreateOrderHeaderResponse>, ProblemHttpResult>> CreateOrderHeader
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
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok, ProblemHttpResult>> SoftDeleteOrderHeader
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

        return TypedResults.Ok();
    }

    [HttpPatch("{id}")]
    [ProducesResponseType<ChangeOrderHeaderStatusResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<ChangeOrderHeaderStatusResponse>, ProblemHttpResult>> ChangeOrderHeaderStatus
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

        return TypedResults.Ok(result.Value);
    }
}

