using Shopway.Domain.Errors;
using Shopway.Domain.Results;
using Shopway.Domain.Entities;
using Shopway.Domain.Abstractions;
using Shopway.Domain.ValueObjects;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Abstractions.Repositories;

namespace Shopway.Application.Features.Products.Queries.GetProductByKey;

internal sealed class GetProductByKeyQueryHandler : IQueryHandler<GetProductByKeyQuery, ProductResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IValidator _validator;

    public GetProductByKeyQueryHandler(IProductRepository productRepository, IValidator validator)
    {
        _productRepository = productRepository;
        _validator = validator;
    }

    public async Task<IResult<ProductResponse>> Handle(GetProductByKeyQuery query, CancellationToken cancellationToken)
    {
        ValidationResult<ProductName> productNameResult = ProductName.Create(query.Key.ProductName);
        ValidationResult<Revision> revisionResult = Revision.Create(query.Key.Revision);

        _validator
            .Validate(productNameResult)
            .Validate(revisionResult);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<ProductResponse>();
        }

        var product = await _productRepository
            .GetByKeyOrDefaultAsync(query.Key, cancellationToken);

        if (product is null)
        {
            return Result.Failure<ProductResponse>(Error.NotFound<Product>(query.Key));
        }

        return product
            .ToResponse()
            .ToResult();
    }
}
