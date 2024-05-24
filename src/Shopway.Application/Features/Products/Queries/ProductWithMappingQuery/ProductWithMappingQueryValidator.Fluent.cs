using FluentValidation;

namespace Shopway.Application.Features.Products.Queries.DynamicProductWithMappingQuery;

internal sealed class ProductWithMappingQueryValidator : AbstractValidator<ProductWithMappingQuery>
{
    public ProductWithMappingQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();
    }
}
