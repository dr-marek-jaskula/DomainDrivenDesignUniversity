using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopway.Application.Batch.Products;
using static Shopway.Application.Batch.BatchEntryStatus;

namespace Shopway.Presentation.Controllers;

public partial class ProductsController
{
    [HttpPost("batch/upsert")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductBatchUpsertResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> ProductsBatchUpsert(
        [FromBody] ProductBatchUpsertCommand command,
        CancellationToken cancellationToken)
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