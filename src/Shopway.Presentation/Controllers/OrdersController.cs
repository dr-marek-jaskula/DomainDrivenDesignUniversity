using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopway.Application.CQRS.Orders.Commands.CreateOrder;
using Shopway.Application.CQRS.Orders.Queries.GetOrderById;
using Shopway.Domain.EntityIds;
using Shopway.Presentation.Abstractions;

namespace Shopway.Presentation.Controllers;

[ApiVersion("0.1", Deprecated = true)]
public sealed class OrdersController : ApiController
{
    public OrdersController(ISender sender)
        : base(sender)
    {
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> GetOrderById([FromRoute] OrderId id, CancellationToken cancellationToken)
    {
        var query = new GetOrderByIdQuery(id);

        var response = await Sender.Send(query, cancellationToken);

        if (response.IsFailure)
        {
            return HandleFailure(response);
        }

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateOrderResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var response = await Sender.Send(command, cancellationToken);

        if (response.IsFailure)
        {
            return HandleFailure(response);
        }

        return CreatedAtActionResult(response, nameof(GetOrderById));
    }
}

