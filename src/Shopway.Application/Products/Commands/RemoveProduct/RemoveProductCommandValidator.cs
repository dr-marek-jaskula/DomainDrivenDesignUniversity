using FluentValidation;

namespace Shopway.Application.Products.Commands.RemoveProduct;

internal sealed class RemoveProductCommandValidator : AbstractValidator<RemoveProductCommand>
{
    public RemoveProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}