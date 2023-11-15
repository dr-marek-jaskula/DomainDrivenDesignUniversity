using Shopway.Domain.Results;
using Shopway.Domain.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Abstractions.Repositories;

namespace Shopway.Application.Features.Orders.Commands.SoftDeleteOrderHeader;

internal sealed class SoftDeleteOrderHeaderCommandHandler(IOrderHeaderRepository orderRepository) 
    : ICommandHandler<SoftDeleteOrderHeaderCommand>
{
    private readonly IOrderHeaderRepository _orderHeaderRepository = orderRepository;

    public async Task<IResult> Handle(SoftDeleteOrderHeaderCommand command, CancellationToken cancellationToken)
    {
        var orderHeader = await _orderHeaderRepository.GetByIdAsync(command.OrderHeaderId, cancellationToken);

        orderHeader.SoftDelete();

        return Result.Success();
    }
}