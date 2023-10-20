using FluentValidation;

namespace Shopway.Application.Features.Products.Queries.GetProductByKey;

internal sealed class GetProductByKeyQueryValidator : AbstractValidator<GetProductByKeyQuery>
{
    public GetProductByKeyQueryValidator()
    {
        RuleFor(x => x.Key).NotNull();
        RuleFor(x => x.Key.ProductName).NotEmpty();
        RuleFor(x => x.Key.Revision).NotEmpty();
    }
}