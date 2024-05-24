using FluentValidation;
using Shopway.Application.Utilities;
using Shopway.Domain.Products.ValueObjects;

namespace Shopway.Application.Features.Products.Queries.GetProductByKey;

internal sealed class GetProductByKeyQueryValidator : AbstractValidator<GetProductByKeyQuery>
{
    public GetProductByKeyQueryValidator()
    {
        RuleFor(x => x.Key)
            .NotNull()
            .DependentRules(() =>
            {
                RuleFor(x => x.Key.ProductName)
                    .MustSatisfyValueObjectValidation(ProductName.Validate);

                RuleFor(x => x.Key.Revision)
                    .MustSatisfyValueObjectValidation(Revision.Validate);
            });
    }
}
