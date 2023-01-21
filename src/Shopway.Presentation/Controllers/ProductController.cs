using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopway.Presentation.Abstractions;
using Shopway.Presentation.Requests.Products;
using Shopway.Domain.StronglyTypedIds;
using Shopway.Application.CQRS.Products.Queries.GetProductById;
using Shopway.Application.CQRS.Products.Commands.CreateProduct;
using Shopway.Application.CQRS.Products.Commands.UpdateProduct;
using Shopway.Application.CQRS.Products.Commands.RemoveProduct;
using Shopway.Application.CQRS.Products.Queries.QueryProduct;
using Shopway.Persistence.Specifications.Products;
using Shopway.Domain.Enums;
using Newtonsoft.Json.Linq;
using System;

namespace Shopway.Presentation.Controllers;

public sealed class ProductController : ApiController
{
    public ProductController(ISender sender)
        : base(sender)
    {
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        Guid id, 
        CancellationToken cancellationToken)
    {
        var query = new GetProductByIdQuery(ProductId.Create(id));

        var response = await Sender.Send(query, cancellationToken);

        return QueryResult(response);
    }

    [HttpGet()]
    public async Task<IActionResult> Query(
        QueryProductRequest request,
        CancellationToken cancellationToken)
    {
        var query = new ProductPageQuery(request.PageNumber, request.PageSize)
        {
            Filter = new ProductFilter()
            {
                ProductName = request.FilterByProductName,
                Revision = request.FilterByRevision,
                Price = request.FilterByPrice,
                UomCode = request.FilterByUomCode,
            }
            //Order = new ProductOrder()
            //{
            //    ByProductName = request.OrderByProductName,
            //    ByRevision = request.FilterByPrice,
            //    ByPrice = request.FilterByProductName,
            //    ByUomCode = request.FilterByUomCode,
            //    ThenByProductName = request.FilterByUomCode,
            //    ThenByRevision = request.FilterByUomCode,
            //    ThenByPrice = request.FilterByUomCode,
            //    ThenByUomCode = request.FilterByUomCode,
            //}
        };

        var response = await Sender.Send(query, cancellationToken);

        return QueryResult(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateProductCommand
        (
            request.ProductName,
            request.Price,
            request.UomCode,
            request.Revision
        );

        var response = await Sender.Send(command, cancellationToken);

        if (response.IsFailure)
        {
            return HandleFailure(response);
        }

        return CreatedAtActionResult(response, nameof(GetById));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateProductRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateProductCommand(ProductId.Create(id), request.Price);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new RemoveProductCommand(ProductId.Create(id));

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }
}