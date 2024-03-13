using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shopway.Application.Features.Orders.Commands.BatchUpsertOrderLine;
using Shopway.Domain.Orders;
using static Shopway.Application.Features.BatchEntryStatus;

namespace Shopway.Presentation.Controllers;

partial class OrderHeadersController
{
    [HttpPost("batch/upsert/{orderHeaderId}")]
    [ProducesResponseType<BatchUpsertOrderLineResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<BatchUpsertOrderLineResponse>, ProblemHttpResult, BadRequest<BatchUpsertOrderLineResponse>>> OrderLinesBatchUpsert
    (
        [FromRoute] OrderHeaderId orderHeaderId,
        [FromBody] BatchUpsertOrderLineCommand command,
        CancellationToken cancellationToken
    )
    {
        var result = await Sender.Send(command with { OrderHeaderId = orderHeaderId }, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        if (result.IsSuccess && result.Value.Entries.Any(entry => entry.Status is Error))
        {
            return TypedResults.BadRequest(result.Value);
        }

        return TypedResults.Ok(result.Value);
    }
}