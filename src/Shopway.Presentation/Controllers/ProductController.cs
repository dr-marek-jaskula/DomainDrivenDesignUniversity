using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopway.Domain.Results;
using Shopway.Presentation.Abstractions;
using Shopway.Presentation.Requests.Products;
using Shopway.Domain.StronglyTypedIds;
using Shopway.Application.CQRS.Products.Queries.GetProductById;
using Shopway.Application.CQRS.Products.Commands.Reviews.UpdateReview;
using Shopway.Application.CQRS.Products.Commands.CreateProduct;
using Shopway.Application.CQRS.Products.Commands.UpdateProduct;
using Shopway.Application.CQRS.Products.Commands.RemoveProduct;
using Shopway.Application.CQRS.Products.Commands.Reviews.AddReview;
using Shopway.Application.CQRS.Products.Commands.Reviews.RemoveReview;

namespace Shopway.Presentation.Controllers;

[Route("api/[controller]")]
public sealed class ProductController : ApiController
{
    public ProductController(ISender sender)
        : base(sender)
    {
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id, 
        CancellationToken cancellationToken)
    {
        var productId = new ProductId() { Value = id };

        var query = new GetProductByIdQuery(productId);

        var response = await Sender.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateProductCommand(
            request.ProductName,
            request.Price,
            request.UomCode,
            request.Revision);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Value },
            result.Value);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateProductRequest request,
        CancellationToken cancellationToken)
    {
        var productId = new ProductId() { Value = id };

        var command = new UpdateProductCommand(productId, request.Price);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Remove(
        Guid id,
        CancellationToken cancellationToken)
    {
        var productId = new ProductId() { Value = id };

        var command = new RemoveProductCommand(productId);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return NoContent();
    }

    [HttpPost("{id:guid}/review")]
    public async Task<IActionResult> AddReview(
        Guid id,
        [FromBody] AddReviewRequest request,
        CancellationToken cancellationToken)
    {
        var productId = new ProductId() { Value = id };

        var command = new AddReviewCommand(productId, request.Username, request.Stars, request.Title, request.Description);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpPatch("{productId:guid}/review/{reviewId:guid}")]
    public async Task<IActionResult> UpdateReview(
        Guid productId,
        Guid reviewId,
        [FromBody] UpdateReviewRequest request,
    CancellationToken cancellationToken)
    {
        var productIdType = new ProductId() { Value = productId };
        var reviewIdType = new ReviewId() { Value = reviewId };

        var command = new UpdateReviewCommand(productIdType, reviewIdType, request.Stars, request.Description);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpDelete("{productId:guid}/review/{reviewId:guid}")]
    public async Task<IActionResult> RemoveReview(
        Guid productId,
        Guid reviewId,
        CancellationToken cancellationToken)
    {
        var productIdType = new ProductId() { Value = productId };
        var reviewIdType = new ReviewId() { Value = reviewId };

        var command = new RemoveReviewCommand(productIdType, reviewIdType);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }
}