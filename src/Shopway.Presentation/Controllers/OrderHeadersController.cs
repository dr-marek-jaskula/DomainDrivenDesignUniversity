using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopway.Application.CQRS.Orders.Commands.ChangeOrderHeaderStatus;
using Shopway.Application.CQRS.Orders.Commands.CreateHeaderOrder;
using Shopway.Application.CQRS.Orders.Queries;
using Shopway.Application.CQRS.Orders.Queries.GetOrderById;
using Shopway.Domain.EntityIds;
using Shopway.Presentation.Abstractions;

namespace Shopway.Presentation.Controllers;

[ApiVersion("0.1", Deprecated = true)]
public sealed partial class OrderHeadersController : ApiController
{
    public OrderHeadersController(ISender sender)
        : base(sender)
    {
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderHeaderResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> GetOrderHeaderById([FromRoute] OrderHeaderId id, CancellationToken cancellationToken)
    {
        var query = new GetOrderHeaderByIdQuery(id);

        var response = await Sender.Send(query, cancellationToken);

        if (response.IsFailure)
        {
            return HandleFailure(response);
        }

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateOrderHeaderResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> CreateOrderHeader
    (
        [FromBody] CreateOrderHeaderCommand command, 
        CancellationToken cancellationToken
    )
    {
        var response = await Sender.Send(command, cancellationToken);

        if (response.IsFailure)
        {
            return HandleFailure(response);
        }

        return CreatedAtActionResult(response, nameof(GetOrderHeaderById));
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ChangeOrderHeaderStatusResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> ChangeOrderHeaderStatus
    (
        [FromRoute] OrderHeaderId id,
        [FromBody] ChangeOrderHeaderStatusCommand.ChangeOrderHeaderStatusRequestBody body, 
        CancellationToken cancellationToken
    )
    {
        var command = new ChangeOrderHeaderStatusCommand(id, body);
        var response = await Sender.Send(command, cancellationToken);

        if (response.IsFailure)
        {
            return HandleFailure(response);
        }

        return Ok(response);
    }
}

