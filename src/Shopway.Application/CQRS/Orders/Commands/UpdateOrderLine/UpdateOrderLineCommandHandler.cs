using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.Results;
using Shopway.Domain.ValueObjects;
using Shopway.Application.Utilities;
using Shopway.Domain.Entities;
using Shopway.Application.CQRS.Orders.Commands.UpdateOrderLine;

namespace Shopway.Application.CQRS.Products.Commands.UpdateReview;

internal sealed class UpdateOrderLineCommandHandler : ICommandHandler<UpdateOrderLineCommand, UpdateOrderLineResponse>
{
    private readonly IOrderHeaderRepository _orderHeaderRepository;
    private readonly IValidator _validator;

    public UpdateOrderLineCommandHandler(IOrderHeaderRepository orderHeaderRepository, IValidator validator)
    {
        _orderHeaderRepository = orderHeaderRepository;
        _validator = validator;
    }

    //It is not preferred to make partial updates, but for tutorial purpose it is done here
    public async Task<IResult<UpdateOrderLineResponse>> Handle(UpdateOrderLineCommand command, CancellationToken cancellationToken)
    {
        var orderHeader = await _orderHeaderRepository.GetByIdWithOrderLineAsync(command.OrderHeaderId, command.OrderLineId, cancellationToken);

        var orderLineToUpdate = orderHeader
            .OrderLines
            .First(x => x.Id == command.OrderLineId);

        ValidationResult<Amount> amountResult = Amount.Create(command.Body.Amount);
        ValidationResult<Discount> discountResult = Discount.Create(command.Body.Discount ?? 0);

        _validator
            .Validate(amountResult)
            .Validate(discountResult);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<UpdateOrderLineResponse>();
        }

        Update(orderLineToUpdate, amountResult.Value, discountResult.Value);

        return orderLineToUpdate
            .ToUpdateResponse()
            .ToResult();
    }

    private static void Update(OrderLine orderLineToUpdate, Amount amount, Discount discount)
    {
        orderLineToUpdate.UpdateAmount(amount);
        orderLineToUpdate.UpdateDiscount(discount);
    }
}