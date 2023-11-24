using Shopway.Domain.Errors;
using Shopway.Domain.Products;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Results;
using Shopway.Application.Abstractions;
using Shopway.Domain.Products.ValueObjects;
using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Products.Queries.GetProductByKey;

internal sealed class GetProductByKeyQueryHandler(IProductRepository productRepository, IValidator validator)
    : IQueryHandler<GetProductByKeyQuery, ProductResponse>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IValidator _validator = validator;

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
