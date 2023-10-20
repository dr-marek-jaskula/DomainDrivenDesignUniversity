using Shopway.Domain.Abstractions;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Abstractions.Repositories;

namespace Shopway.Application.Features.Orders.Commands.ChangeOrderHeaderStatus;

internal sealed class ChangeOrderHeaderStatusCommandHandler : ICommandHandler<ChangeOrderHeaderStatusCommand, ChangeOrderHeaderStatusResponse>
{
    private readonly IOrderHeaderRepository _orderHeaderRepository;

    public ChangeOrderHeaderStatusCommandHandler(IOrderHeaderRepository orderHeaderRepository)
    {
        _orderHeaderRepository = orderHeaderRepository;
    }

    public async Task<IResult<ChangeOrderHeaderStatusResponse>> Handle(ChangeOrderHeaderStatusCommand command, CancellationToken cancellationToken)
    {
        var orderHeader = await _orderHeaderRepository.GetByIdAsync(command.OrderHeaderId, cancellationToken);

        var statusChangeResult = orderHeader.ChangeStatus(command.Body.NewOrderHeaderStatus);

        if (statusChangeResult.IsFailure)
        {
            return statusChangeResult.Failure<ChangeOrderHeaderStatusResponse>();
        }

        return orderHeader
            .ToChangeStatusResponse()
            .ToResult();
    }
}