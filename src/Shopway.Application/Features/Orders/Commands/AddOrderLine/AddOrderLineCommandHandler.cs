using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Orders;
using Shopway.Domain.Orders.ValueObjects;
using Shopway.Domain.Products;

namespace Shopway.Application.Features.Orders.Commands.AddOrderLine;

//Option without using my own IValidator (for tutorial purposes). Due to the fact the most of the validation is moved to Fluent Validation
//and that this approach does not create an Error instance if not needed (and IValidator does - extra cost), this approach may be 
//preferable. I leave it to viewer decision.
internal sealed class AddOrderLineCommandHandler(IOrderHeaderRepository orderRepository, IProductRepository productRepository)
    : ICommandHandler<AddOrderLineCommand, AddOrderLineResponse>
{
    private readonly IOrderHeaderRepository _orderHeaderRepository = orderRepository;
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<IResult<AddOrderLineResponse>> Handle(AddOrderLineCommand command, CancellationToken cancellationToken)
    {
        var discount = Discount.Create(command.Body.Discount ?? 0).Value;
        var amount = Amount.Create(command.Body.Amount).Value;

        OrderHeader orderHeader = await _orderHeaderRepository
            .GetByIdAsync(command.OrderHeaderId, cancellationToken);

        Product product = await _productRepository
            .GetByIdAsync(command.ProductId, cancellationToken);

        OrderLine createdOrderLine = CreateOrderLine(product, command.OrderHeaderId, amount, discount);

        var addOrderLineResult = orderHeader.AddOrderLine(createdOrderLine);

        if (addOrderLineResult.IsFailure)
        {
            return addOrderLineResult.ToValidationResult<AddOrderLineResponse>();
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
