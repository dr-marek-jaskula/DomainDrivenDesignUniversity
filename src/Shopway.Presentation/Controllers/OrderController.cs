using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopway.Application.CQRS.Orders.Commands.CreateOrder;
using Shopway.Application.CQRS.Orders.Queries.GetOrderById;
using Shopway.Domain.EntityIds;
using Shopway.Presentation.Abstractions;

namespace Shopway.Presentation.Controllers;

public sealed class OrderController : ApiController
{
    public OrderController(ISender sender)
        : base(sender)
    {
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(
        Guid id, 
        CancellationToken cancellationToken)
    {
        var orderId = OrderId.Create(id);

        var query = new GetOrderByIdQuery(orderId);

        var response = await Sender.Send(query, cancellationToken);

        if (response.IsFailure)
        {
            return HandleFailure(response);
        }

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(
        [FromBody] CreateOrderCommand command,
        CancellationToken cancellationToken)
    {
        var response = await Sender.Send(command, cancellationToken);

        if (response.IsFailure)
        {
            return HandleFailure(response);
        }

        return CreatedAtActionResult(response, nameof(GetOrderById));
    }
}

