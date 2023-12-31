using Shopway.Domain.Orders;
using Shopway.Domain.Entities;
using Shopway.Application.Mappings;
using Shopway.Domain.Common.Results;
using Shopway.Application.Utilities;
using Shopway.Application.Abstractions;
using Shopway.Domain.Orders.ValueObjects;
using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Orders.Commands.CreateHeaderOrder;

internal sealed class CreateOrderHeaderCommandHandler(IOrderHeaderRepository orderRepository, IValidator validator) 
    : ICommandHandler<CreateOrderHeaderCommand, CreateOrderHeaderResponse>
{
    private readonly IOrderHeaderRepository _orderHeaderRepository = orderRepository;
    private readonly IValidator _validator = validator;

    public Task<IResult<CreateOrderHeaderResponse>> Handle(CreateOrderHeaderCommand command, CancellationToken cancellationToken)
    {
        ValidationResult<Discount> discountResult = Discount.Create(command.Discount ?? 0);

        _validator
            .Validate(discountResult);

        if (_validator.IsInvalid)
        {
            return _validator
                .Failure<CreateOrderHeaderResponse>()
                .ToResult()
                .ToTask();
        }

        OrderHeader createdOrderHeader = CreateOrderHeader(command, discountResult);

        return createdOrderHeader
            .ToCreateResponse()
            .ToResult()
            .ToTask();
    }

    private OrderHeader CreateOrderHeader(CreateOrderHeaderCommand command, Result<Discount> discountResult)
    {
        var orderToCreate = OrderHeader.Create
        (
            id: OrderHeaderId.New(),
            userId: command.UserId,
            discount: discountResult.Value
        );

        _orderHeaderRepository.Create(orderToCreate);

        return orderToCreate;
    }
}