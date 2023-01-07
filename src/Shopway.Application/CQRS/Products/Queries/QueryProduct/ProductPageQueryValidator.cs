using FluentValidation;
using Shopway.Application.Abstractions;
using Shopway.Domain.Enums;
using static Shopway.Application.CQRS.Products.Queries.QueryProduct.ProductPageQuery;

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

    private static bool ValidateProductPageOrder(ProductOrder productPageOrder)
    {
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