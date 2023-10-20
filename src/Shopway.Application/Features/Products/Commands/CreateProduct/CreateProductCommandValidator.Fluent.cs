using FluentValidation;

namespace Shopway.Application.Features.Products.Commands.CreateProduct;

internal sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.ProductKey).NotNull();
        RuleFor(x => x.ProductKey.ProductName).NotEmpty();
        RuleFor(x => x.ProductKey.Revision).NotEmpty();
        RuleFor(x => x.Price).NotNull();
        RuleFor(x => x.UomCode).NotNull();
    }
}