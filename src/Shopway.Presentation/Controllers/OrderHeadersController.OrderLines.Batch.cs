using Shopway.Domain.EntityIds;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Shopway.Application.CQRS.BatchEntryStatus;
using Shopway.Application.CQRS.Orders.Commands.BatchUpsertOrderLine;

namespace Shopway.Presentation.Controllers;

partial class OrderHeadersController
{
    [HttpPost("batch/upsert/{orderHeaderId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BatchUpsertOrderLineResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
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