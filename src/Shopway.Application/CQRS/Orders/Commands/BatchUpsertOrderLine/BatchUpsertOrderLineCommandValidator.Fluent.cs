using FluentValidation;

namespace Shopway.Application.CQRS.Orders.Commands.BatchUpsertOrderLine;

internal sealed class BatchUpsertOrderLineCommandFluentValidator : AbstractValidator<BatchUpsertOrderLineCommand>
{
    public BatchUpsertOrderLineCommandFluentValidator()
    {
        RuleForEach(command => command.Requests).ChildRules(request =>
        {
            request.RuleFor(x => x.OrderLineKey).NotNull();
        });
    }
}