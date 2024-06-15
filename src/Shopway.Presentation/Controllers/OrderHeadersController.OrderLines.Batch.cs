using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shopway.Application.Features.Orders.Commands.BatchUpsertOrderLine;
using Shopway.Application.Utilities;
using Shopway.Domain.Orders;
using Shopway.Presentation.Utilities;

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
            return result.ToProblemHttpResult();
        }

        if (result.Value.AnyErrorEntry())
        {
            return result.ToBadRequestResult();
        }

        return result.ToOkResult();
    }
}
