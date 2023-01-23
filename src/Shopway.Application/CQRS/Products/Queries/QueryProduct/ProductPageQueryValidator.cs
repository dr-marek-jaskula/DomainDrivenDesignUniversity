using FluentValidation;
using Shopway.Application.Constants;
using Shopway.Domain.Enums;
using Shopway.Persistence.Specifications.Products;

namespace Shopway.Application.CQRS.Products.Queries.QueryProduct;

internal sealed class ProductPageQueryValidator : AbstractValidator<ProductPageQuery>
{
    public ProductPageQueryValidator()
    {
        RuleFor(query => query.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(query => query.PageSize).Custom((pageSize, context) =>
        {
            if (PageConstants.AllowedPageSizes.Contains(pageSize) is false)
            { 
                context.AddFailure($"PageSize", $"PageSize must be in: [{string.Join(",", PageConstants.AllowedPageSizes)}]");
            }
        });

        RuleFor(query => query.Order)
            .Must(ValidateProductPageOrder)
            .WithMessage("Invalid SortBy or ThenBy: Single SortBy can be select and if so, single ThenBy can be chosen");
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
            .Count(x => x is not null);

        var thenByArray = new SortDirection?[]
        {
            productPageOrder.ThenByProductName,
            productPageOrder.ThenByRevision,
            productPageOrder.ThenByPrice,
            productPageOrder.ThenByUomCode,
        };

        var thenByNotNullCount = thenByArray
            .Count(x => x is not null);

        if (sortByNotNullCount > 1 || thenByNotNullCount > 1)
        {
            return false;
        }
        else if (sortByNotNullCount is 0 && thenByNotNullCount is 1)
        {
            return false;
        }

        return true;
    }
}