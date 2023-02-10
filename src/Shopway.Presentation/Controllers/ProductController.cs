using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopway.Presentation.Abstractions;
using Shopway.Domain.EntityIds;
using Shopway.Application.CQRS.Products.Queries.GetProductById;
using Shopway.Application.CQRS.Products.Commands.CreateProduct;
using Shopway.Application.CQRS.Products.Commands.UpdateProduct;
using Shopway.Application.CQRS.Products.Commands.RemoveProduct;
using Shopway.Application.CQRS.Products.Queries.QueryProduct;
using Shopway.Application.Batch.Products;
using Shopway.Infrastructure.Authentication.ApiKeyAuthentication;
using static Shopway.Application.Batch.BatchEntryStatus;

namespace Shopway.Presentation.Controllers;

public sealed class ProductController : ApiController
{
    public ProductController(ISender sender)
        : base(sender)
    {
    }

    [HttpGet("{id}")]
    [ApiKey(RequiredApiKeyName.PRODUCT_GET)]
    public async Task<IActionResult> GetById(
        [FromRoute] GetProductByIdQuery query,
        CancellationToken cancellationToken)
    {
        var response = await Sender.Send(query, cancellationToken);

        if (response.IsFailure)
        {
            return HandleFailure(response);
        }

        return Ok(response);
    }

    [HttpGet()]
    public async Task<IActionResult> Query(
        [FromBody] ProductPageQuery query,
        CancellationToken cancellationToken)
    {
        var response = await Sender.Send(query, cancellationToken);

        if (response.IsFailure)
        {
            return HandleFailure(response);
        }

        return Ok(response);
    }

    [HttpPost]
    [ApiKey(RequiredApiKeyName.PRODUCT_CREATE)]
    public async Task<IActionResult> Create(
        [FromBody] CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        var response = await Sender.Send(command, cancellationToken);

        if (response.IsFailure)
        {
            return HandleFailure(response);
        }

        return CreatedAtActionResult(response, nameof(GetById));
    }

    [HttpPut("{id}")]
    [ApiKey(RequiredApiKeyName.PRODUCT_UPDATE)]
    public async Task<IActionResult> Update(
        [FromRoute] ProductId id,
        [FromBody] UpdateProductCommand.UpdateRequestBody body,
        CancellationToken cancellationToken)
    {
        var command = new UpdateProductCommand(id, body);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    [ApiKey(RequiredApiKeyName.PRODUCT_REMOVE)]
    public async Task<IActionResult> Remove(
        [FromRoute] RemoveProductCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }


    [HttpPost("batch/upsert")]
    public async Task<IActionResult> BatchUpsert(
        [FromBody] ProductBatchUpsertCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        if (result.IsSuccess && result.Value.Entries.Any(entry => entry.Status is Error))
        {
            return BadRequest(result.Value);
        }

        return Ok(result.Value);
    }
}