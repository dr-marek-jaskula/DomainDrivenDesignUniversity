using FluentValidation;

namespace Shopway.Application.Products.Commands.CreateProduct;

internal sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.ProductName).NotEmpty();

        RuleFor(x => x.Price).NotEmpty().GreaterThan(0);

        RuleFor(x => x.UomCode).NotEmpty();

        RuleFor(x => x.Revision).NotEmpty();
    }
}