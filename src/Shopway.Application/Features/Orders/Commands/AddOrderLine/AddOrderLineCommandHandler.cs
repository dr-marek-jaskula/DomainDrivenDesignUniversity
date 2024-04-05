using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Features.Orders.Commands.AddOrderLine;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Orders;
using Shopway.Domain.Orders.ValueObjects;
using Shopway.Domain.Products;

namespace Shopway.Application.Features.Orders.Commands.CreateHeaderOrder;

internal sealed class AddOrderLineCommandHandler(IOrderHeaderRepository orderRepository, IProductRepository productRepository, IValidator validator)
    : ICommandHandler<AddOrderLineCommand, AddOrderLineResponse>
{
    private readonly IOrderHeaderRepository _orderHeaderRepository = orderRepository;
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IValidator _validator = validator;

    public async Task<IResult<AddOrderLineResponse>> Handle(AddOrderLineCommand command, CancellationToken cancellationToken)
    {
        ValidationResult<Discount> discountResult = Discount.Create(command.Body.Discount ?? 0);
        ValidationResult<Amount> amountResult = Amount.Create(command.Body.Amount);

        _validator
            .Validate(discountResult)
            .Validate(amountResult);

        if (_validator.IsInvalid)
        {
            return _validator
                .Failure<AddOrderLineResponse>()
                .ToResult();
        }

        OrderHeader orderHeader = await _orderHeaderRepository
            .GetByIdAsync(command.OrderHeaderId, cancellationToken);

        Product product = await _productRepository
            .GetByIdAsync(command.ProductId, cancellationToken);

        OrderLine createdOrderLine = CreateOrderLine(product, command.OrderHeaderId, amountResult.Value, discountResult.Value);

        var addOrderLineResult = orderHeader.AddOrderLine(createdOrderLine);

        _validator
            .Validate(addOrderLineResult);

        if (addOrderLineResult.IsFailure)
        {
            return _validator
                .Failure<AddOrderLineResponse>()
                .ToResult();
        }

        return createdOrderLine
            .ToAddResponse()
            .ToResult();
    }

    private static OrderLine CreateOrderLine(Product product, OrderHeaderId orderHeaderId, Amount amount, Discount lineDiscount)
    {
        return OrderLine.Create
        (
            OrderLineId.New(),
            product.ToSummary(),
            orderHeaderId,
            amount,
            lineDiscount
        );
    }
}