using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Shopway.Application.CQRS.BatchEntryStatus;
using Shopway.Application.CQRS.Orders.Commands.BatchUpsertOrderLine;

namespace Shopway.Presentation.Controllers;

partial class OrderHeadersController
{
    [HttpPost("batch/upsert")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BatchUpsertOrderLineResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> OrderLinesBatchUpsert([FromBody] BatchUpsertOrderLineCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

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