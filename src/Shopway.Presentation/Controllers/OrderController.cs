using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopway.Application.CQRS.Orders.Commands.CreateOrder;
using Shopway.Application.CQRS.Orders.Queries.GetOrderById;
using Shopway.Domain.StronglyTypedIds;
using Shopway.Presentation.Abstractions;
using Shopway.Presentation.Requests.Orders;

namespace Shopway.Presentation.Controllers;

[Route("api/[controller]")]
public sealed class OrderController : ApiController
{
    public OrderController(ISender sender)
        : base(sender)
    {
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOrderById(
        Guid id, 
        CancellationToken cancellationToken)
    {
        var orderId = OrderId.New(id);

        var query = new GetOrderByIdQuery(orderId);

        var response = await Sender.Send(query, cancellationToken);

        return QueryResult(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(
        [FromBody] CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateOrderCommand(
            request.ProductId,
            request.Amount,
            request.CustomerId,
            request.Discount);

        var response = await Sender.Send(command, cancellationToken);

        if (response.IsFailure)
        {
            return HandleFailure(response);
        }

        return CreatedAtActionResult(response, nameof(GetOrderById));
    }
}

