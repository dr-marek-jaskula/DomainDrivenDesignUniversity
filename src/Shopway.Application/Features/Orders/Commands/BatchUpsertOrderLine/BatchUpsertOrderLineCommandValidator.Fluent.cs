using FluentValidation;

namespace Shopway.Application.Features.Orders.Commands.BatchUpsertOrderLine;

internal sealed class BatchUpsertOrderLineCommandFluentValidator : AbstractValidator<BatchUpsertOrderLineCommand>
{
    public BatchUpsertOrderLineCommandFluentValidator()
    {
        RuleForEach(command => command.Requests).ChildRules(request =>
        {
            request.RuleFor(x => x.ProductId).NotNull().NotEmpty();
        });
    }
}