using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Features.Orders.Commands.UpdateOrderLine;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Orders;
using Shopway.Domain.Orders.ValueObjects;

namespace Shopway.Application.Features.Products.Commands.UpdateOrderLine;

internal sealed class UpdateOrderLineCommandHandler(IOrderHeaderRepository orderHeaderRepository)
    : ICommandHandler<UpdateOrderLineCommand, UpdateOrderLineResponse>
{
    private readonly IOrderHeaderRepository _orderHeaderRepository = orderHeaderRepository;

    //It is not preferred to make partial updates, but for tutorial purpose it is done here
    public async Task<IResult<UpdateOrderLineResponse>> Handle(UpdateOrderLineCommand command, CancellationToken cancellationToken)
    {
        var orderHeader = await _orderHeaderRepository.GetByIdWithOrderLineAsync(command.OrderHeaderId, command.OrderLineId, cancellationToken);

        var orderLineToUpdate = orderHeader
            .OrderLines
            .First(x => x.Id == command.OrderLineId);

        orderLineToUpdate.UpdateAmount(Amount.Create(command.Body.Amount).Value);

        if (command.Body.Discount is decimal discount)
        {
            orderLineToUpdate.UpdateDiscount(Discount.Create(discount).Value);
        }

        return orderLineToUpdate
            .ToUpdateResponse()
            .ToResult();
    }
}
