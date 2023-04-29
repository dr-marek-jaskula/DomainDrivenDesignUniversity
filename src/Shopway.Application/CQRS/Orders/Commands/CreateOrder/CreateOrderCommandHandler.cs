using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.Entities;
using Shopway.Domain.Results;
using Shopway.Domain.EntityIds;
using Shopway.Domain.ValueObjects;
using Shopway.Application.Utilities;
using Shopway.Domain.Utilities;

namespace Shopway.Application.CQRS.Orders.Commands.CreateOrder;

internal sealed class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, CreateOrderResponse>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IValidator _validator;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IValidator validator)
    {
        _orderRepository = orderRepository;
        _validator = validator;
    }

    public Task<IResult<CreateOrderResponse>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        ValidationResult<Amount> amountResult = Amount.Create(command.Amount);
        ValidationResult<Discount> discountResult = Discount.Create(command.Discount ?? 0);

        _validator
            .Validate(amountResult)
            .Validate(discountResult);

        if (_validator.IsInvalid)
        {
            return _validator
                .Failure<CreateOrderResponse>()
                .ToResult()
                .ToTask();
        }

        Order createdOrder = CreateOrder(command, amountResult, discountResult);

        return createdOrder
            .ToCreateResponse()
            .ToResult()
            .ToTask();
    }

    private Order CreateOrder(CreateOrderCommand command, Result<Amount> amountResult, Result<Discount> discountResult)
    {
        var orderToCreate = Order.Create
        (
            id: OrderId.New(),
            productId: ProductId.Create(command.ProductId),
            amount: amountResult.Value,
            customerId: PersonId.Create(command.CustomerId),
            discount: discountResult.Value
        );

        _orderRepository.Create(orderToCreate);

        return orderToCreate;
    }
}