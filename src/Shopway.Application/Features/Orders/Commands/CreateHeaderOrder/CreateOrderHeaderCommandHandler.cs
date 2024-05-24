using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Orders;
using Shopway.Domain.Orders.ValueObjects;

namespace Shopway.Application.Features.Orders.Commands.CreateHeaderOrder;

internal sealed class CreateOrderHeaderCommandHandler(IOrderHeaderRepository orderRepository)
    : ICommandHandler<CreateOrderHeaderCommand, CreateOrderHeaderResponse>
{
    private readonly IOrderHeaderRepository _orderHeaderRepository = orderRepository;

    public Task<IResult<CreateOrderHeaderResponse>> Handle(CreateOrderHeaderCommand command, CancellationToken cancellationToken)
    {
        var discount = Discount.Create(command.Discount ?? 0).Value;

        OrderHeader createdOrderHeader = CreateOrderHeader(command, discount);

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
