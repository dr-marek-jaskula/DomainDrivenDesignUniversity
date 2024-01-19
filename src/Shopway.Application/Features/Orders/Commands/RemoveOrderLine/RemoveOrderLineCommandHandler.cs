using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Features.Orders.Commands.RemoveOrderLine;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Orders;

namespace Shopway.Application.Features.Products.Commands.RemoveReview;

internal sealed class RemoveOrderLineCommandHandler(IOrderHeaderRepository orderHeaderRepository)
    : ICommandHandler<RemoveOrderLineCommand, RemoveOrderLineResponse>
{
    private readonly IOrderHeaderRepository _orderHeaderRepository = orderHeaderRepository;

    public async Task<IResult<RemoveOrderLineResponse>> Handle(RemoveOrderLineCommand command, CancellationToken cancellationToken)
    {
        var orderHeader = await _orderHeaderRepository.GetByIdWithOrderLineAsync(command.OrderHeaderId, command.OrderLineId, cancellationToken);

        var orderLineToRemove = orderHeader
            .OrderLines
            .First(x => x.Id == command.OrderLineId);

        orderHeader.RemoveOrderLine(orderLineToRemove);

        return orderLineToRemove
            .ToRemoveResponse()
            .ToResult();
    }
}
