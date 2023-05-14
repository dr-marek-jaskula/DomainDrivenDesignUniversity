using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Shopway.Application.CQRS.BatchEntryStatus;

namespace Shopway.Presentation.Controllers;

partial class OrderHeadersController
{
    [HttpPost("batch/insert")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BatchInsertOrderLineResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> OrderLinesBatchInsert([FromBody] BatchInsertOrderLineCommand command, CancellationToken cancellationToken)
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