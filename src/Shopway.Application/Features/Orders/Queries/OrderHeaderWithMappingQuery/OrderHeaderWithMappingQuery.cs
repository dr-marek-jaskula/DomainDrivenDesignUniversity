using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Orders;
using Shopway.Domain.Orders.DataProcessing.Mapping;

namespace Shopway.Application.Features.Orders.Queries;

public sealed record OrderHeaderWithMappingQuery(OrderHeaderId OrderHeaderId) : IQuery<DataTransferObjectResponse>
{
    public OrderHeaderDynamicMapping? Mapping { get; init; }
}
