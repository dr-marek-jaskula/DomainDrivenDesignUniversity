using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopway.Presentation.Abstractions;
using Shopway.Presentation.Requests.Products;
using Shopway.Domain.StronglyTypedIds;
using Shopway.Application.CQRS.Products.Commands.Reviews.UpdateReview;
using Shopway.Application.CQRS.Products.Commands.Reviews.AddReview;
using Shopway.Application.CQRS.Products.Commands.Reviews.RemoveReview;

namespace Shopway.Presentation.Controllers;

[Route("api/{productId}/[controller]")]
public sealed class ReviewController : ApiController
{
    public ReviewController(ISender sender)
        : base(sender)
    {
    }

    [HttpPost()]
    public async Task<IActionResult> Add(
        Guid productId,
        [FromBody] AddReviewRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddReviewCommand
        (
            ProductId.Create(productId), 
            request.Username, 
            request.Stars, 
            request.Title, 
            request.Description
        );

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpPatch("{reviewId}")]
    public async Task<IActionResult> Update(
        Guid productId,
        Guid reviewId,
        [FromBody] UpdateReviewRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateReviewCommand
        (
            ProductId.Create(productId),
            ReviewId.Create(reviewId), 
            request.Stars, 
            request.Description
        );

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpDelete("{reviewId}")]
    public async Task<IActionResult> Remove(
        Guid productId,
        Guid reviewId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveReviewCommand
        (
            ProductId.Create(productId),
            ReviewId.Create(reviewId)
        );

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }
}