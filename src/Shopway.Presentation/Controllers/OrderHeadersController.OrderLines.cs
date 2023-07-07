using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopway.Application.CQRS.Orders.Commands.AddOrderLine;
using Shopway.Application.CQRS.Orders.Commands.RemoveOrderLine;
using Shopway.Application.CQRS.Orders.Commands.UpdateOrderLine;
using Shopway.Domain.EntityIds;

namespace Shopway.Presentation.Controllers;

partial class OrderHeadersController
{
    public const string OrderLines = nameof(OrderLines);

    [HttpPost("{orderHeaderId}/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddOrderLineResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> AddReview
    (
        [FromRoute] OrderHeaderId orderHeaderId,
        [FromRoute] ProductId productId,
        [FromBody] AddOrderLineCommand.AddOrderLineRequestBody body,
        CancellationToken cancellationToken
    )
    {
        var command = new AddOrderLineCommand(orderHeaderId, productId, body);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result);
    }

    [HttpDelete($"{{orderHeaderId}}/{OrderLines}/{{orderLineId}}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RemoveOrderLineResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> RemoveReview
    (
        [FromRoute] OrderHeaderId orderHeaderId,
        [FromRoute] OrderLineId orderLineId,
        CancellationToken cancellationToken
    )
    {
        var command = new RemoveOrderLineCommand(orderHeaderId, orderLineId);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result);
    }

    [HttpPut($"{{orderHeaderId}}/{OrderLines}/{{orderLineId}}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateOrderLineResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> UpdateReview
    (
        [FromRoute] OrderHeaderId orderHeaderId,
        [FromRoute] OrderLineId orderLineId,
        [FromBody] UpdateOrderLineCommand.UpdateOrderLineRequestBody body,
        CancellationToken cancellationToken
    )
    {
        var command = new UpdateOrderLineCommand(orderHeaderId, orderLineId, body);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result);
    }
}