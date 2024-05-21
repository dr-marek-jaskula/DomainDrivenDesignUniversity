using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shopway.Application.Features.Products.Commands.BatchUpsertProduct;
using Shopway.Application.Utilities;
using Shopway.Presentation.Abstractions;

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
            return result.ToProblemHttpResult();
        }

        if (result.Value.AnyErrorEntry())
        {
            return result.ToBadRequestResult();
        }

        return result.ToOkResult();
    }
}