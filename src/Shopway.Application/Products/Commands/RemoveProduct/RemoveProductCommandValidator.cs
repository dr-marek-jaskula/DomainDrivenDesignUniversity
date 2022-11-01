using FluentValidation;

namespace Shopway.Application.Products.Commands.RemoveProduct;

//TODO check if validation is automatic!?
internal sealed class RemoveProductCommandValidator : AbstractValidator<RemoveProductCommand>
{
    public RemoveProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}