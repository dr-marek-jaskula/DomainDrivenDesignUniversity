using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopway.Application.Products.Commands.AddReview;
using Shopway.Application.Products.Commands.CreateProduct;
using Shopway.Application.Products.Commands.RemoveReview;
using Shopway.Application.Products.Commands.RemoveProduct;
using Shopway.Application.Products.Commands.UpdateProduct;
using Shopway.Application.Products.Queries.GetProductById;
using Shopway.Domain.Results;
using Shopway.Presentation.Abstractions;
using Shopway.Presentation.Requests.Products;

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
        var query = new GetProductByIdQuery(id);

        Result<ProductResponse> response = await Sender.Send(query, cancellationToken);

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

        Result<Guid> result = await Sender.Send(command, cancellationToken);

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
        var command = new UpdateProductCommand(id, request.Price);

        Result<Guid> result = await Sender.Send(command, cancellationToken);

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
        var command = new RemoveProductCommand(id);

        Result<Guid> result = await Sender.Send(command, cancellationToken);

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
        var command = new AddReviewCommand(id, request.Username, request.Stars, request.Title, request.Description);

        Result<Guid> result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    //[HttpPatch("{id:guid}/review")]
    //public async Task<IActionResult> UpdateReview(
    //Guid id,
    //[FromBody] AddReviewRequest request,
    //CancellationToken cancellationToken)
    //{
    //    var command = new AddReviewCommand(id, request.Username, request.Stars, request.Title, request.Description);

    //    Result<Guid> result = await Sender.Send(command, cancellationToken);

    //    if (result.IsFailure)
    //    {
    //        return HandleFailure(result);
    //    }

    //    return Ok(result.Value);
    //}

    [HttpDelete("{productId:guid}/review/{reviewId:guid}")]
    public async Task<IActionResult> RemoveReview(
    Guid productId,
    Guid reviewId,
    CancellationToken cancellationToken)
    {
        var command = new RemoveReviewCommand(productId, reviewId);

        Result<Guid> result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }
}