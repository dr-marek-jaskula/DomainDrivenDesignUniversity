using Shopway.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Shopway.Domain.EntityIds;
using Shopway.Infrastructure.Authentication;
using Shopway.Application.CQRS.Products.Commands.AddReview;
using Shopway.Application.CQRS.Products.Commands.RemoveReview;
using Shopway.Application.CQRS.Products.Commands.UpdateReview;
using Microsoft.AspNetCore.Http;

namespace Shopway.Presentation.Controllers;

partial class ProductsController
{
    public const string Reviews = nameof(Reviews);

    [HttpPost($"{{productId}}/{Reviews}")]
    [HasPermission(Permission.CRUD_Review)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddReviewResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> AddReview(
            [FromRoute] ProductId productId,
            [FromBody] AddReviewCommand.AddReviewRequestBody body,
            CancellationToken cancellationToken)
    {
        var command = new AddReviewCommand(productId, body);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }


    [HttpPatch($"{{productId}}/{Reviews}/{{reviewId}}")]
    [HasPermission(Permission.CRUD_Review)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateReviewResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> UpdateReview(
        [FromRoute] ProductId productId,
        [FromRoute] ReviewId reviewId,
        [FromBody] UpdateReviewCommand.UpdateReviewRequestBody body,
        CancellationToken cancellationToken)
    {
        var command = new UpdateReviewCommand(productId, reviewId, body);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpDelete($"{{productId}}/{Reviews}/{{reviewId}}")]
    [HasPermission(Permission.CRUD_Review)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RemoveReviewResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> RemoveReview(
        [FromRoute] RemoveReviewCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }
}