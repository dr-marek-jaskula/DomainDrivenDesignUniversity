﻿using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Products;

namespace Shopway.Application.Features.Products.Queries.GetProductById;

internal sealed class GetProductByIdQueryHandler(IProductRepository productRepository)
    : IQueryHandler<GetProductByIdQuery, ProductResponse>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<IResult<ProductResponse>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await _productRepository
            .GetByIdAsync(query.Id, cancellationToken);

        return product
            .ToResponse()
            .ToResult();
    }
}
