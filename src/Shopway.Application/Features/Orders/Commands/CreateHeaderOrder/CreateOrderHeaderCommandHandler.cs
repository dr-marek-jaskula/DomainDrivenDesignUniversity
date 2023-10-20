using Shopway.Domain.Results;
using Shopway.Domain.Entities;
using Shopway.Domain.Utilities;
using Shopway.Domain.EntityIds;
using Shopway.Domain.Abstractions;
using Shopway.Domain.ValueObjects;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Abstractions.Repositories;

namespace Shopway.Application.Features.Orders.Commands.CreateHeaderOrder;

internal sealed class CreateOrderHeaderCommandHandler : ICommandHandler<CreateOrderHeaderCommand, CreateOrderHeaderResponse>
{
    private readonly IOrderHeaderRepository _orderHeaderRepository;
    private readonly IValidator _validator;

    public CreateOrderHeaderCommandHandler(IOrderHeaderRepository orderRepository, IValidator validator)
    {
        _orderHeaderRepository = orderRepository;
        _validator = validator;
    }

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