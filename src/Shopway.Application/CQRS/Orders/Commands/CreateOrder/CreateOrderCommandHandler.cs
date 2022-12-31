using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Entities;
using Shopway.Domain.Errors;
using Shopway.Domain.Repositories;
using Shopway.Domain.Results;
using Shopway.Domain.ValueObjects;

namespace Shopway.Application.CQRS.Orders.Commands.CreateOrder;

internal sealed class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, CreateOrderResponse>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult<CreateOrderResponse>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        Result<Amount> amountResult = Amount.Create(command.Amount);
        Result<Discount> discountResult = Discount.Create(command.Discount ?? 0);

        Error error = ErrorHandler.FirstValueObjectErrorOrErrorNone(amountResult, discountResult);

        if (error != Error.None)
        {
            return Result.Failure<CreateOrderResponse>(error);
        }

        var order = Order.Create(
            Guid.NewGuid(),
            command.ProductId,
            amountResult.Value,
            command.CustomerId,
            discountResult.Value);

        _orderRepository.Create(order);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new CreateOrderResponse(order.Id.Value);

        return Result.Create(response);
    }
}