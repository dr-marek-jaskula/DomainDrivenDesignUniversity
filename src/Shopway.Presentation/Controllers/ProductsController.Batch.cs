using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shopway.Application.Features.Products.Commands.BatchUpsertProduct;
using static Shopway.Application.Features.BatchEntryStatus;

namespace Shopway.Presentation.Controllers;

public partial class ProductsController
{
    [HttpPost("batch/upsert")]
    [ProducesResponseType<BatchUpsertProductResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<BatchUpsertProductResponse>, ProblemHttpResult, BadRequest<BatchUpsertProductResponse>>> ProductsBatchUpsert
    (
        [FromBody] BatchUpsertProductCommand command, 
        CancellationToken cancellationToken
    )
    {
        var result = await Sender.Send(command, cancellationToken);

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