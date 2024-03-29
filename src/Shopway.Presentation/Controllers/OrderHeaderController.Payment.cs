using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shopway.Application.Features.Orders.Commands.FinalizePaymentProcess;
using Shopway.Application.Features.Orders.Commands.StartPaymentProcess;
using Shopway.Domain.Orders;

namespace Shopway.Presentation.Controllers;

partial class OrderHeadersController
{
    public const string Webhook = nameof(Webhook);
    public const string Payment = nameof(Payment);

    [HttpPost($"{{id}}/{Payment}/start")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok, ProblemHttpResult>> StartPaymentProcess
    (
        [FromRoute] OrderHeaderId id,
        [FromBody] StartPaymentProcessCommand.StartPaymentProcessCommandBody body,
        CancellationToken cancellationToken
    )
    {
        var command = new StartPaymentProcessCommand(id, body);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return TypedResults.Ok();
    }

    [HttpPost($"{Payment}/{Webhook}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok, ProblemHttpResult>> PaymentWebhook
    (
        [FromBody] FinalizePaymentProcessCommand command,
        CancellationToken cancellationToken
    )
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return TypedResults.Ok();
    }
}