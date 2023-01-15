using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Abstractions;
using Shopway.Domain.Entities;
using Shopway.Domain.Results;
using Shopway.Application.Mapping;
using static Shopway.Domain.Errors.HttpErrors;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.Abstractions;

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

        var response = order!.ToResponse();

        return Result.Create(response);
    }
}