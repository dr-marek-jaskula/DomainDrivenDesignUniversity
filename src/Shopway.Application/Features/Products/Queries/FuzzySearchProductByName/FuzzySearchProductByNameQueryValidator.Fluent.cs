using FluentValidation;
using Shopway.Application.Abstractions;
using Shopway.Domain.Common.DataProcessing;

namespace Shopway.Application.Features.Products.Queries.FuzzySearchProductByName;

internal sealed class FuzzySearchProductByNameQueryValidator : OffsetPageQueryValidator<FuzzySearchProductByNameQuery, ProductResponse, OffsetPage>
{
    public FuzzySearchProductByNameQueryValidator()
    {
        RuleFor(x => x.ProductName)
            .NotNull()
            .NotEmpty();
    }
}
