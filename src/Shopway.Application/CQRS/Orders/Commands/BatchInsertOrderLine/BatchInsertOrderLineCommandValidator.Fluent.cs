using FluentValidation;

namespace Shopway.Application.CQRS.Orders.Commands.BatchInsertOrderLine;

internal sealed class BatchInsertOrderLineCommandFluentValidator : AbstractValidator<BatchInsertOrderLineCommand>
{
    public BatchInsertOrderLineCommandFluentValidator()
    {
        RuleForEach(command => command.Requests).ChildRules(request =>
        {
            request.RuleFor(x => x.OrderLineKey).NotNull();
            request.RuleFor(x => x.Amount).GreaterThan(0);
            request.RuleFor(x => x.Discount).GreaterThanOrEqualTo(0);
        });
    }
}