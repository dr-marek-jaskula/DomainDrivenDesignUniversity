﻿using Shopway.Domain.EntityIds;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Shopway.Application.Features.Orders.Commands.AddOrderLine;
using Shopway.Application.Features.Orders.Commands.RemoveOrderLine;
using Shopway.Application.Features.Orders.Commands.UpdateOrderLine;

namespace Shopway.Presentation.Controllers;

partial class OrderHeadersController
{
    public const string OrderLines = nameof(OrderLines);
    public const string Products = nameof(Products);

    [HttpPost($"{{orderHeaderId}}/{Products}/{{productId}}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddOrderLineResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
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

        return Ok(result);
    }

    [HttpDelete($"{{orderHeaderId}}/{OrderLines}/{{orderLineId}}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RemoveOrderLineResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
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

        return Ok(result);
    }

    [HttpPut($"{{orderHeaderId}}/{OrderLines}/{{orderLineId}}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateOrderLineResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
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

        return Ok(result);
    }
}