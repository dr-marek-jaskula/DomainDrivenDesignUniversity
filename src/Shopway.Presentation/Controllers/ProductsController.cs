using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopway.Application.Features;
using Shopway.Application.Features.Products.Commands.CreateProduct;
using Shopway.Application.Features.Products.Commands.RemoveProduct;
using Shopway.Application.Features.Products.Commands.UpdateProduct;
using Shopway.Application.Features.Products.Queries;
using Shopway.Application.Features.Products.Queries.DynamicOffsetProductQuery;
using Shopway.Application.Features.Products.Queries.FuzzySearchProductByName;
using Shopway.Application.Features.Products.Queries.GetProductById;
using Shopway.Application.Features.Products.Queries.GetProductByKey;
using Shopway.Application.Features.Products.Queries.GetProductsCursorDictionary;
using Shopway.Application.Features.Products.Queries.GetProductsOffsetDictionary;
using Shopway.Application.Features.Products.Queries.QueryOffsetPageProduct;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.EntityKeys;
using Shopway.Domain.Products;
using Shopway.Presentation.Abstractions;
using Shopway.Presentation.Authentication.ApiKeyAuthentication;

namespace Shopway.Presentation.Controllers;

public sealed partial class ProductsController(ISender sender) : ApiController(sender)
{
    /// <summary>
    /// Gets product by specified id
    /// </summary>
    /// <remarks>This documentation is for tutorial purpose - to demonstrate how to provide the OpenApi documentation</remarks>
    /// <param name="id">Product id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Product</returns>
    [HttpGet("{id}")]
    [RequiredApiKey(RequiredApiKey.PRODUCT_GET)]
    [ProducesResponseType<ProductResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetProductById([FromRoute] ProductId id, CancellationToken cancellationToken)
    {
        var query = new GetProductByIdQuery(id);

        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Gets product by specified key
    /// </summary>
    /// <param name="key">Product key</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Product</returns>
    [HttpGet("key")]
    [RequiredApiKey(RequiredApiKey.PRODUCT_GET)]
    [ProducesResponseType<ProductResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetProductByKey([FromBody] ProductKey key, CancellationToken cancellationToken)
    {
        var query = new GetProductByKeyQuery(key);

        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Fuzzy search product by name
    /// </summary>
    /// <param name="productName">Product name</param>
    /// <param name="page">Offset page</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Product</returns>
    [HttpPost("fuzzy-search/{productName}")]
    [RequiredApiKey(RequiredApiKey.PRODUCT_GET)]
    [ProducesResponseType<ProductResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> FuzzySearchProductByName
    (
        [FromRoute] string productName,
        [FromBody] OffsetPage page,
        CancellationToken cancellationToken
    )
    {
        var query = new FuzzySearchProductByNameQuery(page, productName);

        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpPost("name/{productNamePattern}")]
    [ProducesResponseType<OffsetPageResponse<DictionaryResponseEntry>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> QueryProductsByNameLike
    (
        [FromRoute] string productNamePattern,
        [FromBody] OffsetPage page,
        CancellationToken cancellationToken
    )
    {
        var query = new GetProductByNameLikePageQuery(page, productNamePattern);

        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpPost("query/dictionary/offset")]
    [ProducesResponseType<OffsetPageResponse<DictionaryResponseEntry>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> QueryProductsOffsetDictionary([FromBody] ProductDictionaryOffsetPageQuery query, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpPost("query/dictionary/cursor")]
    [ProducesResponseType<CursorPageResponse<DictionaryResponseEntry>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> QueryProductsCursorDictionary([FromBody] ProductDictionaryCursorPageQuery query, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    //These static or dynamic suffixes are only for tutorial purpose
    [HttpPost("query/static")]
    [ProducesResponseType<OffsetPageResponse<ProductResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> StaticQueryProducts([FromBody] ProductOffsetPageQuery query, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpPost("query/dynamic")]
    [ProducesResponseType<OffsetPageResponse<ProductResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DynamicQueryProducts([FromBody] ProductOffsetPageDynamicQuery query, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpPost("query/dynamic/with-mapping")]
    [ProducesResponseType<OffsetPageResponse<ProductResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DynamicQueryProductsWithMapping([FromBody] ProductOffsetPageDynamicWithMappingQuery query, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpPost]
    [RequiredApiKey(RequiredApiKey.PRODUCT_CREATE)]
    [ProducesResponseType<CreateProductResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return CreatedAtActionResult(result, nameof(GetProductById));
    }

    [HttpPut("{id}")]
    [RequiredApiKey(RequiredApiKey.PRODUCT_UPDATE)]
    [ProducesResponseType<UpdateProductResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
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
    [RequiredApiKey(RequiredApiKey.PRODUCT_REMOVE)]
    [ProducesResponseType<RemoveProductResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
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