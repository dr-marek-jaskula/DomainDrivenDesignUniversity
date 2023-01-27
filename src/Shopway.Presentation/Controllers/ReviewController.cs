using MediatR;
using Shopway.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Shopway.Presentation.Abstractions;
using Shopway.Domain.EntityIds;
using Shopway.Application.CQRS.Products.Commands.Reviews.AddReview;
using Shopway.Application.CQRS.Products.Commands.Reviews.RemoveReview;
using Shopway.Infrastructure.Authentication;
using Shopway.Application.CQRS.Products.Commands.Reviews.UpdateReview;

namespace Shopway.Presentation.Controllers;

[Route("api/Product/{productId}/[controller]")]
public sealed class ReviewController : ApiController
{
    public ReviewController(ISender sender)
        : base(sender)
    {
    }

    [HttpPost()]
    [HasPermission(Permission.CRUD_Review)]
    public async Task<IActionResult> Add(
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

    [HttpPatch("{reviewId}")]
    [HasPermission(Permission.CRUD_Review)]
    public async Task<IActionResult> Update(
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

    [HttpDelete("{reviewId}")]
    [HasPermission(Permission.CRUD_Review)]
    public async Task<IActionResult> Remove(
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