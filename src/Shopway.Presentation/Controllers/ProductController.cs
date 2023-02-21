using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopway.Presentation.Abstractions;
using Shopway.Domain.EntityIds;
using Shopway.Application.CQRS.Products.Queries.GetProductById;
using Shopway.Application.CQRS.Products.Commands.CreateProduct;
using Shopway.Application.CQRS.Products.Commands.UpdateProduct;
using Shopway.Application.CQRS.Products.Commands.RemoveProduct;
using Shopway.Application.CQRS.Products.Queries.QueryProduct;
using Shopway.Infrastructure.Authentication.ApiKeyAuthentication;

namespace Shopway.Presentation.Controllers;

public sealed partial class ProductController : ApiController
{
    public ProductController(ISender sender)
        : base(sender)
    {
    }

    [HttpGet("{id}")]
    [ApiKey(RequiredApiKeyName.PRODUCT_GET)]
    public async Task<IActionResult> GetProductById(
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
    public async Task<IActionResult> QueryProduct(
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
    public async Task<IActionResult> CreateProduct(
        [FromBody] CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        var response = await Sender.Send(command, cancellationToken);

        if (response.IsFailure)
        {
            return HandleFailure(response);
        }

        return CreatedAtActionResult(response, nameof(GetProductById));
    }

    [HttpPut("{id}")]
    [ApiKey(RequiredApiKeyName.PRODUCT_UPDATE)]
    public async Task<IActionResult> UpdateProduct(
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
    public async Task<IActionResult> RemoveProduct(
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
}