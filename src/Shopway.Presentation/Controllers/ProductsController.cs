using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shopway.Application.Features;
using Shopway.Application.Features.Products.Commands.CreateProduct;
using Shopway.Application.Features.Products.Commands.RemoveProduct;
using Shopway.Application.Features.Products.Commands.UpdateProduct;
using Shopway.Application.Features.Products.Queries;
using Shopway.Application.Features.Products.Queries.DynamicOffsetProductQuery;
using Shopway.Application.Features.Products.Queries.DynamicOffsetProductWithMappingQuery;
using Shopway.Application.Features.Products.Queries.FuzzySearchProductByName;
using Shopway.Application.Features.Products.Queries.GetProductById;
using Shopway.Application.Features.Products.Queries.GetProductByKey;
using Shopway.Application.Features.Products.Queries.GetProductsCursorDictionary;
using Shopway.Application.Features.Products.Queries.GetProductsOffsetDictionary;
using Shopway.Application.Features.Products.Queries.QueryOffsetPageProduct;
using Shopway.Application.Features.Products.Queries.QueryOffsetPageProductWithMapping;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.EntityKeys;
using Shopway.Domain.Products;
using Shopway.Presentation.Abstractions;
using Shopway.Presentation.Authentication.ApiKeyAuthentication;
using Shopway.Presentation.Utilities;

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
    [RequiredApiKey<ApiKeyName>(ApiKeyName.PRODUCT_GET)]
    [ProducesResponseType<ProductResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<ProductResponse>, ProblemHttpResult>> GetProductById
    (
        [FromRoute] ProductId id,
        CancellationToken cancellationToken
    )
    {
        var query = new GetProductByIdQuery(id);

        var result = await Sender.Send(query, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    /// <summary>
    /// Gets product by specified key
    /// </summary>
    /// <param name="key">Product key</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Product</returns>
    [HttpGet("key")]
    [RequiredApiKey<ApiKeyName>(ApiKeyName.PRODUCT_GET)]
    [ProducesResponseType<ProductResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<ProductResponse>, ProblemHttpResult>> GetProductByKey
    (
        [FromBody] ProductKey key,
        CancellationToken cancellationToken
    )
    {
        var query = new GetProductByKeyQuery(key);

        var result = await Sender.Send(query, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    /// <summary>
    /// Fuzzy search product by name
    /// </summary>
    /// <param name="productName">Product name</param>
    /// <param name="page">Offset page</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Product</returns>
    [HttpPost("fuzzy-search/{productName}")]
    [RequiredApiKey<ApiKeyName>(ApiKeyName.PRODUCT_GET)]
    [ProducesResponseType<ProductResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<OffsetPageResponse<ProductResponse>>, ProblemHttpResult>> FuzzySearchProductByName
    (
        [FromRoute] string productName,
        [FromBody] OffsetPage page,
        CancellationToken cancellationToken
    )
    {
        var query = new FuzzySearchProductByNameQuery(page, productName);

        var result = await Sender.Send(query, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    [HttpPost("name/{productNamePattern}")]
    [ProducesResponseType<OffsetPageResponse<DictionaryResponseEntry<ProductKey>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<OffsetPageResponse<ProductResponse>>, ProblemHttpResult>> QueryProductsByNameLike
    (
        [FromRoute] string productNamePattern,
        [FromBody] OffsetPage page,
        CancellationToken cancellationToken
    )
    {
        var query = new GetProductByNameLikePageQuery(page, productNamePattern);

        var result = await Sender.Send(query, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    [HttpPost("query/dictionary/offset")]
    [ProducesResponseType<OffsetPageResponse<DictionaryResponseEntry<ProductKey>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<OffsetPageResponse<DictionaryResponseEntry<ProductKey>>>, ProblemHttpResult>> QueryProductsOffsetDictionary
    (
        [FromBody] ProductDictionaryOffsetPageQuery query,
        CancellationToken cancellationToken
    )
    {
        var result = await Sender.Send(query, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    [HttpPost("query/dictionary/cursor")]
    [ProducesResponseType<CursorPageResponse<DictionaryResponseEntry<ProductKey>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<CursorPageResponse<DictionaryResponseEntry<ProductKey>>>, ProblemHttpResult>> QueryProductsCursorDictionary
    (
        [FromBody] ProductDictionaryCursorPageQuery query,
        CancellationToken cancellationToken
    )
    {
        var result = await Sender.Send(query, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    //These static or dynamic suffixes are only for tutorial purpose
    [HttpPost("query/static")]
    [ProducesResponseType<OffsetPageResponse<ProductResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<OffsetPageResponse<ProductResponse>>, ProblemHttpResult>> StaticQueryProducts
    (
        [FromBody] ProductOffsetPageQuery query,
        CancellationToken cancellationToken
    )
    {
        var result = await Sender.Send(query, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    //These with-mapping suffixes are only for tutorial purpose
    [HttpPost("query/static/with-mapping")]
    [ProducesResponseType<OffsetPageResponse<DataTransferObjectResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<OffsetPageResponse<DataTransferObjectResponse>>, ProblemHttpResult>> StaticQueryProductsWithMapping
    (
        [FromBody] ProductOffsetPageWithMappingQuery query,
        CancellationToken cancellationToken
    )
    {
        var result = await Sender.Send(query, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    [HttpPost("query/dynamic")]
    [ProducesResponseType<OffsetPageResponse<ProductResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<OffsetPageResponse<ProductResponse>>, ProblemHttpResult>> DynamicQueryProducts
    (
        [FromBody] ProductOffsetPageDynamicQuery query,
        CancellationToken cancellationToken
    )
    {
        var result = await Sender.Send(query, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    [HttpPost("query/dynamic/with-mapping")]
    [ProducesResponseType<OffsetPageResponse<DataTransferObjectResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<OffsetPageResponse<DataTransferObjectResponse>>, ProblemHttpResult>> DynamicQueryProductsWithMapping
    (
        [FromBody] ProductOffsetPageDynamicWithMappingQuery query,
        CancellationToken cancellationToken
    )
    {
        var result = await Sender.Send(query, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    [HttpPost]
    [RequiredApiKey<ApiKeyName>(ApiKeyName.PRODUCT_CREATE)]
    [ProducesResponseType<CreateProductResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<CreateProductResponse>, ProblemHttpResult>> CreateProduct
    (
        [FromBody] CreateProductCommand command,
        CancellationToken cancellationToken
    )
    {
        var result = await Sender.Send(command, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    [HttpPut("{id}")]
    [RequiredApiKey<ApiKeyName>(ApiKeyName.PRODUCT_UPDATE)]
    [ProducesResponseType<UpdateProductResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<UpdateProductResponse>, ProblemHttpResult>> UpdateProduct
    (
        [FromRoute] ProductId id,
        [FromBody] UpdateProductCommand.UpdateRequestBody body,
        CancellationToken cancellationToken
    )
    {
        var command = new UpdateProductCommand(id, body);

        var result = await Sender.Send(command, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    [HttpDelete("{id}")]
    [RequiredApiKey<ApiKeyName>(ApiKeyName.PRODUCT_REMOVE)]
    [ProducesResponseType<RemoveProductResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<RemoveProductResponse>, ProblemHttpResult>> RemoveProduct
    (
        [FromRoute] ProductId id,
        CancellationToken cancellationToken
    )
    {
        var command = new RemoveProductCommand(id);

        var result = await Sender.Send(command, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }
}
