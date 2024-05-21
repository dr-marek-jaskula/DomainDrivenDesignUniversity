using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shopway.Application.Features.Orders.Commands.AddOrderLine;
using Shopway.Application.Features.Orders.Commands.RemoveOrderLine;
using Shopway.Application.Features.Orders.Commands.UpdateOrderLine;
using Shopway.Domain.Orders;
using Shopway.Domain.Products;
using Shopway.Presentation.Abstractions;

namespace Shopway.Presentation.Controllers;

partial class OrderHeadersController
{
    public const string OrderLines = nameof(OrderLines);
    public const string Products = nameof(Products);

    [HttpPost($"{{orderHeaderId}}/{Products}/{{productId}}")]
    [ProducesResponseType<AddOrderLineResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<AddOrderLineResponse>, ProblemHttpResult>> AddOrderLine
    (
        [FromRoute] OrderHeaderId orderHeaderId,
        [FromRoute] ProductId productId,
        [FromBody] AddOrderLineCommand.AddOrderLineRequestBody body,
        CancellationToken cancellationToken
    )
    {
        var command = new AddOrderLineCommand(orderHeaderId, productId, body);

        var result = await Sender.Send(command, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    [HttpDelete($"{{orderHeaderId}}/{OrderLines}/{{orderLineId}}")]
    [ProducesResponseType<RemoveOrderLineResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<RemoveOrderLineResponse>, ProblemHttpResult>> RemoveOrderLine
    (
        [FromRoute] OrderHeaderId orderHeaderId,
        [FromRoute] OrderLineId orderLineId,
        CancellationToken cancellationToken
    )
    {
        var command = new RemoveOrderLineCommand(orderHeaderId, orderLineId);

        var result = await Sender.Send(command, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    [HttpPut($"{{orderHeaderId}}/{OrderLines}/{{orderLineId}}")]
    [ProducesResponseType<UpdateOrderLineResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<UpdateOrderLineResponse>, ProblemHttpResult>> UpdateOrderLine
    (
        [FromRoute] OrderHeaderId orderHeaderId,
        [FromRoute] OrderLineId orderLineId,
        [FromBody] UpdateOrderLineCommand.UpdateOrderLineRequestBody body,
        CancellationToken cancellationToken
    )
    {
        var command = new UpdateOrderLineCommand(orderHeaderId, orderLineId, body);

        var result = await Sender.Send(command, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }
}