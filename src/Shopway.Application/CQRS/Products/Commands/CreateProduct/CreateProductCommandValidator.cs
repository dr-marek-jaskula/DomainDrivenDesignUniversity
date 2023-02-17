using FluentValidation;

namespace Shopway.Application.CQRS.Products.Commands.CreateProduct;

internal sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.ProductName).NotNull();
        RuleFor(x => x.Price).NotNull();
        RuleFor(x => x.UomCode).NotNull();
        RuleFor(x => x.Revision).NotNull();
    }
}