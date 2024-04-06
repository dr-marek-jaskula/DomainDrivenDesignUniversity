using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shopway.Application.Features.Orders.Commands.CancelPaymentProcess;
using Shopway.Application.Features.Orders.Commands.FinalizePaymentProcess;
using Shopway.Application.Features.Orders.Commands.RefundPaymentProcess;
using Shopway.Application.Features.Orders.Commands.StartPaymentProcess;
using Shopway.Domain.Orders;

namespace Shopway.Presentation.Controllers;

partial class OrderHeadersController
{
    public const string Webhook = nameof(Webhook);
    public const string Payments = nameof(Payments);

    [HttpPost($"{{id}}/{Payments}/start")]
    [ProducesResponseType<StartPaymentProcessResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<StartPaymentProcessResponse>, ProblemHttpResult>> StartPaymentProcess
    (
        [FromRoute] OrderHeaderId id,
        CancellationToken cancellationToken
    )
    {
        var command = new StartPaymentProcessCommand(id);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return TypedResults.Ok(result.Value);
    }

    [HttpPost($"{Payments}/{Webhook}/success")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok, ProblemHttpResult>> PaymentSuccessWebhook(CancellationToken cancellationToken)
    {
        var result = await Sender.Send(FinalizePaymentProcessCommand.Instance, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return TypedResults.Ok();
    }

    [HttpPost($"{Payments}/{Webhook}/cancel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok, ProblemHttpResult>> PaymentCancelWebhook(CancellationToken cancellationToken)
    {
        var result = await Sender.Send(CancelPaymentProcessCommand.Instance, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return TypedResults.Ok();
    }

    [HttpPost($"{{id}}/{Payments}/{{paymentId}}/refund")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok, ProblemHttpResult>> PaymentRefund
    (
        [FromRoute] OrderHeaderId id,
        [FromRoute] PaymentId paymentId,
        CancellationToken cancellationToken
    )
    {
        var result = await Sender.Send(new RefundPaymentProcessCommand(id, paymentId), cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return TypedResults.Ok();
    }
}