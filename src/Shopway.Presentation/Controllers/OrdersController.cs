using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopway.Application.CQRS.Orders.Commands.CreateOrder;
using Shopway.Application.CQRS.Orders.Queries.GetOrderById;
using Shopway.Presentation.Abstractions;

namespace Shopway.Presentation.Controllers;

public sealed class OrdersController : ApiController
{
    public OrdersController(ISender sender)
        : base(sender)
    {
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(
        [FromRoute] GetOrderByIdQuery query, 
        CancellationToken cancellationToken)
    {
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

