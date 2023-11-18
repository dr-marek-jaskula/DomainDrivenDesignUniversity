using Shopway.Domain.Results;
using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Domain.ValueObjects;
using Shopway.Domain.Abstractions;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Application.Features.Orders.Commands.AddOrderLine;

namespace Shopway.Application.Features.Orders.Commands.CreateHeaderOrder;

internal sealed class AddOrderLineCommandHandler(IOrderHeaderRepository orderRepository, IValidator validator) 
    : ICommandHandler<AddOrderLineCommand, AddOrderLineResponse>
{
    private readonly IOrderHeaderRepository _orderHeaderRepository = orderRepository;
    private readonly IValidator _validator = validator;

    public async Task<IResult<AddOrderLineResponse>> Handle(AddOrderLineCommand command, CancellationToken cancellationToken)
    {
        ValidationResult<Discount> discountResult = Discount.Create(command.Body.Discount ?? 0);
        ValidationResult<Amount> amountResult = Amount.Create(command.Body.Amount);

        _validator
            .Validate(discountResult)
            .Validate(amountResult);

        if (_validator.IsInvalid)
        {
            return _validator
                .Failure<AddOrderLineResponse>()
                .ToResult();
        }

        OrderHeader orderHeader = await _orderHeaderRepository
            .GetByIdAsync(command.OrderHeaderId, cancellationToken);

        OrderLine createdOrderLine = CreateOrderLine(command.ProductId, command.OrderHeaderId, amountResult.Value, discountResult.Value);

        orderHeader.AddOrderLine(createdOrderLine);

        return createdOrderLine
            .ToAddResponse()
            .ToResult();
    }

    private static OrderLine CreateOrderLine(ProductId productId, OrderHeaderId orderHeaderId, Amount amount, Discount lineDiscount)
    {
        return OrderLine.Create
        (
            OrderLineId.New(),
            productId,
            orderHeaderId,
            amount,
            lineDiscount
        );
    }
}