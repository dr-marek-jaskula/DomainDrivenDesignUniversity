using Shopway.Domain.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.Results;

namespace Shopway.Application.CQRS.Orders.Commands.DeleteOrderHeader;

internal sealed class SoftDeleteOrderHeaderCommandHandler : ICommandHandler<SoftDeleteOrderHeaderCommand>
{
    private readonly IOrderHeaderRepository _orderHeaderRepository;

    public SoftDeleteOrderHeaderCommandHandler(IOrderHeaderRepository orderRepository)
    {
        _orderHeaderRepository = orderRepository;
    }

    public async Task<IResult> Handle(SoftDeleteOrderHeaderCommand command, CancellationToken cancellationToken)
    {
        var orderHeader = await _orderHeaderRepository.GetByIdAsync(command.OrderHeaderId, cancellationToken);

        orderHeader.SoftDelete();

        return Result.Success();
    }
}