using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Entities;
using Shopway.Domain.Enums;
using Shopway.Domain.Repositories;
using Shopway.Domain.Results;
using Shopway.Domain.ValueObjects;

namespace Shopway.Application.Orders.Commands.CreateOrder;

internal sealed class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        Result<Amount> amountResult = Amount.Create(command.Amount);
        Result<Discount> discountResult = Discount.Create(command.Discount ?? 0);

        var order = Order.Create(
            Guid.NewGuid(),
            command.ProductId,
            amountResult.Value,
            command.CustomerId,
            discountResult.Value);

        _orderRepository.Create(order);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return order.Id;
    }
}