using Shopway.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Shopway.Domain.EntityIds;
using Shopway.Infrastructure.Authentication;
using Shopway.Application.CQRS.Products.Commands.AddReview;
using Shopway.Application.CQRS.Products.Commands.RemoveReview;
using Shopway.Application.CQRS.Products.Commands.UpdateReview;

namespace Shopway.Presentation.Controllers;

partial class ProductController
{
    private const string Review = nameof(Review);

    [HttpPost("{productId}/" + Review)]
    [HasPermission(Permission.CRUD_Review)]
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


    [HttpPatch("{productId}/" + Review + "/{reviewId}")]
    [HasPermission(Permission.CRUD_Review)]
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

    [HttpDelete("{productId}/" + Review + "/{reviewId}")]
    [HasPermission(Permission.CRUD_Review)]
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