using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mapping;
using Shopway.Domain.Entities;
using Shopway.Domain.Repositories;
using Shopway.Domain.Results;
using Shopway.Domain.StronglyTypedIds;
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
            return _validator.Failure<CreateOrderResponse>();
        }

        var orderToCreate = Order.Create(
            OrderId.New(),
            ProductId.New(command.ProductId),
            amountResult.Value,
            PersonId.New(command.CustomerId),
            discountResult.Value);

        _orderRepository.Create(orderToCreate);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = orderToCreate.ToCreateResponse();

        return Result.Create(response);
    }
}