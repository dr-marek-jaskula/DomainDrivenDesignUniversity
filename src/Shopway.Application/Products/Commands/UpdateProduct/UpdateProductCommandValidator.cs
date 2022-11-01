using FluentValidation;

namespace Shopway.Application.Products.Commands.UpdateProduct;

internal sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => new { x.ProductName, x.Price, x.UomCode, x.Revision })
            .Must(x => 
                x.ProductName is not null 
                && x.Price is not null
                && x.UomCode is not null
                && x.Revision is not null);
    }
}