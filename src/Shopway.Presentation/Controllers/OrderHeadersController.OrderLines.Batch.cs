using Shopway.Domain.EntityIds;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Shopway.Application.Features.Orders.Commands.BatchUpsertOrderLine;
using static Shopway.Application.Features.BatchEntryStatus;

namespace Shopway.Presentation.Controllers;

partial class OrderHeadersController
{
    [HttpPost("batch/upsert/{orderHeaderId}")]
    [ProducesResponseType<BatchUpsertOrderLineResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> OrderLinesBatchUpsert
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
            return BadRequest(result.Value);
        }

        return Ok(result.Value);
    }
}