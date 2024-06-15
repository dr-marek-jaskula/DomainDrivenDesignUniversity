using Shopway.Application.Abstractions;
namespace Shopway.Application.Features.Orders.Commands.CreateHeaderOrder;

public sealed record CreateOrderHeaderResponse
(
    Ulid Id
) : IResponse;
