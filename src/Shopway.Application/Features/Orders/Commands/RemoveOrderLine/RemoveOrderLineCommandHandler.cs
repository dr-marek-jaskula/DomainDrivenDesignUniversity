using Shopway.Domain.Abstractions;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Application.Features.Orders.Commands.RemoveOrderLine;

namespace Shopway.Application.Features.Products.Commands.RemoveReview;

internal sealed class RemoveOrderLineCommandHandler : ICommandHandler<RemoveOrderLineCommand, RemoveOrderLineResponse>
{
    private readonly IOrderHeaderRepository _orderHeaderRepository;

    public RemoveOrderLineCommandHandler(IOrderHeaderRepository orderHeaderRepository)
    {
        _orderHeaderRepository = orderHeaderRepository;
    }

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
