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
using Microsoft.AspNetCore.Http;
using Shopway.Application.CQRS.Products.Queries;
using Shopway.Application.CQRS;
using Shopway.Domain.EntityKeys;
using Shopway.Application.CQRS.Products.Queries.GetProductByKey;
using Shopway.Application.CQRS.Products.Queries.GetProductsDictionary;
using Shopway.Application.CQRS.Products.Queries.QueryProductByExpression;

namespace Shopway.Presentation.Controllers;

public sealed partial class ProductsController : ApiController
{
    public ProductsController(ISender sender)
        : base(sender)
    {
    }

    /// <summary>
    /// Gets product by specified id
    /// </summary>
    /// <remarks>This documentation is for tutorial purpose - to demonstrate how to provide the OpenApi documentation</remarks>
    /// <param name="id">Product id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Product</returns>
    [HttpGet("{id}")]
    [ApiKey(RequiredApiKey.PRODUCT_GET)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> GetProductById([FromRoute] ProductId id, CancellationToken cancellationToken)
    {
        var query = new GetProductByIdQuery(id);

        var response = await Sender.Send(query, cancellationToken);

        if (response.IsFailure)
        {
            return HandleFailure(response);
        }

        return Ok(response);
    }

    /// <summary>
    /// Gets product by specified key
    /// </summary>
    /// <param name="key">Product key</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Product</returns>
    [HttpGet("key")]
    [ApiKey(RequiredApiKey.PRODUCT_GET)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> GetProductByKey([FromBody] ProductKey key, CancellationToken cancellationToken)
    {
        var query = new GetProductByKeyQuery(key);

        var response = await Sender.Send(query, cancellationToken);

        if (response.IsFailure)
        {
            return HandleFailure(response);
        }

        return Ok(response);
    }

    [HttpPost("query/dictionary")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PageResponse<DictionaryResponseEntry>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> QueryProductsDictionary([FromBody] ProductDictionaryPageQuery query, CancellationToken cancellationToken)
    {
        var response = await Sender.Send(query, cancellationToken);

        if (response.IsFailure)
        {
            return HandleFailure(response);
        }

        return Ok(response);
    }

    //These static or dynamic suffixes are only for tutorial purpose

    [HttpPost("query/static")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PageResponse<ProductResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> StaticQueryProducts([FromBody] ProductPageQuery query, CancellationToken cancellationToken)
    {
        var response = await Sender.Send(query, cancellationToken);

        if (response.IsFailure)
        {
            return HandleFailure(response);
        }

        return Ok(response);
    }

    [HttpPost("query/dynamic")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PageResponse<ProductResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> DynamicQueryProducts([FromBody] ProductPageDynamicQuery query, CancellationToken cancellationToken)
    {
        var response = await Sender.Send(query, cancellationToken);

        if (response.IsFailure)
        {
            return HandleFailure(response);
        }

        return Ok(response);
    }

    [HttpPost]
    [ApiKey(RequiredApiKey.PRODUCT_CREATE)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateProductResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
    {
        var response = await Sender.Send(command, cancellationToken);

        if (response.IsFailure)
        {
            return HandleFailure(response);
        }

        return CreatedAtActionResult(response, nameof(GetProductById));
    }

    [HttpPut("{id}")]
    [ApiKey(RequiredApiKey.PRODUCT_UPDATE)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateProductResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> UpdateProduct
    (
        [FromRoute] ProductId id,
        [FromBody] UpdateProductCommand.UpdateRequestBody body,
        CancellationToken cancellationToken
    )
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
    [ApiKey(RequiredApiKey.PRODUCT_REMOVE)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RemoveProductResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> RemoveProduct([FromRoute] ProductId id, CancellationToken cancellationToken)
    {
        var command = new RemoveProductCommand(id);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }
}