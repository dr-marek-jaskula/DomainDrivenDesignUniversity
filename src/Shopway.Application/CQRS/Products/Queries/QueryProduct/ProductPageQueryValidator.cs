using FluentValidation;
using Shopway.Domain.Enums;
using Shopway.Persistence.Specifications.Products;

namespace Shopway.Application.CQRS.Products.Queries.QueryProduct;

internal sealed class ProductPageQueryValidator : AbstractValidator<ProductPageQuery>
{
    public ProductPageQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .NotEmpty()
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .NotEmpty()
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.Order)
            .Must(ValidateProductPageOrder);
    }

    private static bool ValidateProductPageOrder(ProductOrder? productPageOrder)
    {
        if (productPageOrder is null)
        {
            return true;
        }

        var sortByArray = new SortDirection?[]
        {
            productPageOrder.ByProductName,
            productPageOrder.ByRevision,
            productPageOrder.ByPrice,
            productPageOrder.ByUomCode,
        };

        var sortByNotNullCount = sortByArray
            .Where(x => x is not null)
            .Count();

        var thenByArray = new SortDirection?[]
        {
            productPageOrder.ThenByProductName,
            productPageOrder.ThenByRevision,
            productPageOrder.ThenByPrice,
            productPageOrder.ThenByUomCode,
        };

        var thenByNotNullCount = thenByArray
            .Where(x => x is not null)
            .Count();

        return sortByNotNullCount is 1 
            && thenByNotNullCount is 1;
    }
}