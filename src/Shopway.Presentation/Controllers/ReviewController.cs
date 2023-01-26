using MediatR;
using Shopway.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Shopway.Presentation.Abstractions;
using Shopway.Presentation.Requests.Products;
using Shopway.Domain.EntityIds;
using Shopway.Application.CQRS.Products.Commands.Reviews.UpdateReview;
using Shopway.Application.CQRS.Products.Commands.Reviews.AddReview;
using Shopway.Application.CQRS.Products.Commands.Reviews.RemoveReview;
using Shopway.Infrastructure.Authentication;

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
        Guid productId,
        [FromBody] AddReviewRequest request, //AddReviewCommand.Request
        CancellationToken cancellationToken)
    {
        var command = new AddReviewCommand
        (
            ProductId.Create(productId), 
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
    [HasPermission(Permission.CRUD_Review)]
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
    [HasPermission(Permission.CRUD_Review)]
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