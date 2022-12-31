using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Entities;
using Shopway.Domain.Errors;
using Shopway.Domain.Repositories;
using Shopway.Domain.Results;

namespace Shopway.Application.CQRS.Orders.Queries.GetOrderById;

internal sealed class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, OrderResponse>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByIdQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IResult<OrderResponse>> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(query.Id, cancellationToken);

        if (order is null)
        {
            return Result.Failure<OrderResponse>(HttpErrors.NotFound(nameof(Order), query.Id.Value));
        }

        var response = new OrderResponse(
            Id: order.Id.Value,
            Amount: order.Amount,
            Status: order.Status,
            Product: order.Product,
            Payment: order.Payment,
            Customer: order.Customer);

        return Result.Create(response);
    }
}