using FluentValidation;

namespace Shopway.Application.Batch.Products;

internal sealed class ProductBatchUpsertCommandFleuntValidator : AbstractValidator<ProductBatchUpsertCommand>
{
    public ProductBatchUpsertCommandFleuntValidator()
    {
        RuleForEach(command => command.Requests).ChildRules(request =>
        {
            request.RuleFor(x => x.ProductKey).NotNull();
            request.RuleFor(x => x.ProductKey.ProductName).NotEmpty();
            request.RuleFor(x => x.ProductKey.Revision).NotEmpty();
        });
    }
}