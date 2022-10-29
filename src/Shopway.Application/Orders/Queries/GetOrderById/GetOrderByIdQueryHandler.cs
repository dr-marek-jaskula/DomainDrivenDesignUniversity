using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Errors;
using Shopway.Domain.Repositories;
using Shopway.Domain.Results;

namespace Shopway.Application.Orders.Queries.GetOrderById;

internal sealed class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, OrderResponse>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByIdQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<OrderResponse>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

        if (order is null)
        {
            return Result.Failure<OrderResponse>(new Error(
                "Order.NotFound",
                $"The order with Id: {request.OrderId} was not found"));
        }

        var response = new OrderResponse(
            Id: order.Id, 
            Amount: order.Amount, 
            Status: order.Status, 
            Deadline: order.DeliveriedOn, 
            Product: order.Product, 
            Payment: order.Payment, 
            Customer: order.Customer);

        return response;
    }
}