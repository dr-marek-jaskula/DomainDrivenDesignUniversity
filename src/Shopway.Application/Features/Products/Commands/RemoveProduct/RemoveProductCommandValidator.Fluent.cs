using FluentValidation;

namespace Shopway.Application.Features.Products.Commands.RemoveProduct;

internal sealed class RemoveProductCommandValidator : AbstractValidator<RemoveProductCommand>
{
    public RemoveProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}