using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopway.Domain.EntityIds;
using Microsoft.AspNetCore.Http;
using Shopway.Presentation.Abstractions;
using Shopway.Application.Features.Orders.Queries;
using Shopway.Application.Features.Orders.Queries.GetOrderById;
using Shopway.Application.Features.Orders.Commands.CreateHeaderOrder;
using Shopway.Application.Features.Orders.Commands.SoftDeleteOrderHeader;
using Shopway.Application.Features.Orders.Commands.ChangeOrderHeaderStatus;

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

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> SoftDeleteOrderHeader
    (
        [FromRoute] OrderHeaderId id, 
        CancellationToken cancellationToken
    )
    {
        var command = new SoftDeleteOrderHeaderCommand(id);
        var response = await Sender.Send(command, cancellationToken);

        if (response.IsFailure)
        {
            return HandleFailure(response);
        }

        return Ok();
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

