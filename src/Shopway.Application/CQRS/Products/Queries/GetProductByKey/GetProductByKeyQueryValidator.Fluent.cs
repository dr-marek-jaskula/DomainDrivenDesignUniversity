using FluentValidation;

namespace Shopway.Application.CQRS.Products.Queries.GetProductByKey;

internal sealed class GetProductByKeyQueryValidator : AbstractValidator<GetProductByKeyQuery>
{
    public GetProductByKeyQueryValidator()
    {
        RuleFor(x => x.Key).NotNull();
        RuleFor(x => x.Key.ProductName).NotEmpty();
        RuleFor(x => x.Key.Revision).NotEmpty();
    }
}