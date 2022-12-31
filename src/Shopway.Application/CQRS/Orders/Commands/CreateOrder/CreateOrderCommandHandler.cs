using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.CQRS.Products.Commands.CreateProduct;
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
    private readonly IValidator _validator;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IValidator validator)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<IResult<CreateOrderResponse>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        Result<Amount> amountResult = Amount.Create(command.Amount);
        Result<Discount> discountResult = Discount.Create(command.Discount ?? 0);

        _validator
            .Validate(amountResult)
            .Validate(discountResult);

        if (_validator.IsInvalid)
        {
            return Result.Failure<CreateOrderResponse>(_validator.Error);
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