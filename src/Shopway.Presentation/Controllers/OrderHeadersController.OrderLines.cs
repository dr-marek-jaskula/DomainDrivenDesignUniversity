using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopway.Application.Features.Orders.Commands.AddOrderLine;
using Shopway.Application.Features.Orders.Commands.RemoveOrderLine;
using Shopway.Application.Features.Orders.Commands.UpdateOrderLine;
using Shopway.Domain.Orders;
using Shopway.Domain.Products;

namespace Shopway.Presentation.Controllers;

partial class OrderHeadersController
{
    public const string OrderLines = nameof(OrderLines);
    public const string Products = nameof(Products);

    [HttpPost($"{{orderHeaderId}}/{Products}/{{productId}}")]
    [ProducesResponseType<AddOrderLineResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddOrderLine
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

        return Ok(result.Value);
    }

    [HttpDelete($"{{orderHeaderId}}/{OrderLines}/{{orderLineId}}")]
    [ProducesResponseType<RemoveOrderLineResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveOrderLine
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

        return Ok(result.Value);
    }

    [HttpPut($"{{orderHeaderId}}/{OrderLines}/{{orderLineId}}")]
    [ProducesResponseType<UpdateOrderLineResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateOrderLine
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

        return Ok(result.Value);
    }
}