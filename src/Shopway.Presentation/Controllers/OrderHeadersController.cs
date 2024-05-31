using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shopway.Application.Features.Orders.Commands.ChangeOrderHeaderStatus;
using Shopway.Application.Features.Orders.Commands.CreateHeaderOrder;
using Shopway.Application.Features.Orders.Commands.SoftDeleteOrderHeader;
using Shopway.Application.Features.Orders.Queries;
using Shopway.Application.Features.Orders.Queries.GetOrderById;
using Shopway.Domain.Orders;
using Shopway.Presentation.Abstractions;
using Shopway.Presentation.Authentication.OrderHeaders.OrderHeaderCreatedByUser;
using Shopway.Presentation.Utilities;

namespace Shopway.Presentation.Controllers;

[ApiVersion("0.1", Deprecated = true)]
public sealed partial class OrderHeadersController(ISender sender, IAuthorizationService authorizationService) : ApiController(sender)
{
    private readonly IAuthorizationService _authorizationService = authorizationService;

    [HttpGet("{id}")]
    [ProducesResponseType<OrderHeaderResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<OrderHeaderResponse>, ProblemHttpResult>> GetOrderHeaderById
    (
        [FromRoute] OrderHeaderId id, 
        CancellationToken cancellationToken
    )
    {
        var query = new GetOrderHeaderByIdQuery(id);

        var result = await Sender.Send(query, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    [HttpPost]
    [ProducesResponseType<CreateOrderHeaderResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<CreateOrderHeaderResponse>, ProblemHttpResult>> CreateOrderHeader
    (
        [FromBody] CreateOrderHeaderCommand command,
        CancellationToken cancellationToken
    )
    {
        var result = await Sender.Send(command, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok, ProblemHttpResult, ForbidHttpResult>> SoftDeleteOrderHeader
    (
        [FromRoute] OrderHeaderId id,
        CancellationToken cancellationToken
    )
    {
        var authorizationResult = await _authorizationService.AuthorizeAsync(User, id, OrderHeaderCreatedByUserRequirement.PolicyName);

        if (authorizationResult.Succeeded is false)
        {
            return authorizationResult.ToForbidResult();
        }

        var command = new SoftDeleteOrderHeaderCommand(id);

        var result = await Sender.Send(command, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    [HttpPatch("{id}")]
    [ProducesResponseType<ChangeOrderHeaderStatusResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<ChangeOrderHeaderStatusResponse>, ProblemHttpResult>> ChangeOrderHeaderStatus
    (
        [FromRoute] OrderHeaderId id,
        [FromBody] ChangeOrderHeaderStatusCommand.ChangeOrderHeaderStatusRequestBody body,
        CancellationToken cancellationToken
    )
    {
        var command = new ChangeOrderHeaderStatusCommand(id, body);

        var result = await Sender.Send(command, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }
}

