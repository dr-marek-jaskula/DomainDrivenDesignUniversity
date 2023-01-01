using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopway.Application.CQRS.Orders.Commands.CreateOrder;
using Shopway.Application.CQRS.Orders.Queries.GetOrderById;
using Shopway.Domain.StronglyTypedIds;
using Shopway.Presentation.Abstractions;
using Shopway.Presentation.Requests.Orders;

namespace Shopway.Presentation.Controllers;

[Route("api/order")]
public sealed class OrderController : ApiController
{
    public OrderController(ISender sender)
        : base(sender)
    {
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOrderById(Guid id, CancellationToken cancellationToken)
    {
        var orderId = new OrderId() { Value = id };

        var query = new GetOrderByIdQuery(orderId);

        var response = await Sender.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
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

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return CreatedAtAction(
            nameof(GetOrderById),
            new { id = result.Value },
            result.Value);
    }
}

