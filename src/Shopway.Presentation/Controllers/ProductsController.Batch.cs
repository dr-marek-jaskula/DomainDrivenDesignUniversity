using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopway.Application.CQRS.Products.Commands.BatchUpsertProduct;
using static Shopway.Application.CQRS.BatchEntryStatus;

namespace Shopway.Presentation.Controllers;

public partial class ProductsController
{
    [HttpPost("batch/upsert")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BatchUpsertProductResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> ProductsBatchUpsert([FromBody] BatchUpsertProductCommand command, CancellationToken cancellationToken)
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