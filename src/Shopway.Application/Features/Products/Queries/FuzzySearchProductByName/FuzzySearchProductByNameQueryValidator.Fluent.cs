using FluentValidation;
using Shopway.Domain.Common;
using Shopway.Application.Abstractions;

namespace Shopway.Application.Features.Products.Queries.FuzzySearchProductByName;

internal sealed class FuzzySearchProductByNameQueryValidator : OffsetPageValidator<FuzzySearchProductByNameQuery, ProductResponse, OffsetPage>
{
    public FuzzySearchProductByNameQueryValidator()
    {
        RuleFor(x => x.ProductName).NotNull();
        RuleFor(x => x.ProductName).NotEmpty();
    }
}