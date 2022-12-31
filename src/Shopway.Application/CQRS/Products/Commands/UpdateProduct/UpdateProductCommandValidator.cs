using FluentValidation;

namespace Shopway.Application.CQRS.Products.Commands.UpdateProduct;

internal sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
    }
}