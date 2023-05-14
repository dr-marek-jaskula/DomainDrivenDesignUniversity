using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.Abstractions;
using Shopway.Application.Utilities;

namespace Shopway.Application.CQRS.Orders.Queries.GetOrderById;

internal sealed class GetOrderHeaderByIdQueryHandler : IQueryHandler<GetOrderHeaderByIdQuery, OrderHeaderResponse>
{
    private readonly IOrderHeaderRepository _orderHeaderRepository;

    public GetOrderHeaderByIdQueryHandler(IOrderHeaderRepository orderRepository)
    {
        _orderHeaderRepository = orderRepository;
    }

    public async Task<IResult<OrderHeaderResponse>> Handle(GetOrderHeaderByIdQuery query, CancellationToken cancellationToken)
    {
        var orderHeader = await _orderHeaderRepository
            .GetByIdAsync(query.Id, cancellationToken);

        return orderHeader
            .ToResponse()
            .ToResult();
    }
}