using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Shopway.Presentation.Abstractions;
using Shopway.Application.Features.Orders.Queries;
using Shopway.Application.Features.Users.Commands.LogUser;
using Shopway.Application.Features.Users.Commands.RegisterUser;
using Shopway.Application.Features.Users.Queries.GetUserByUsername;

namespace Gatherly.Presentation.Controllers;

public sealed class UsersController : ApiController
{
    public UsersController(ISender sender)
        : base(sender)
    {
    }

    [HttpPost("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Login([FromBody] LogUserCommand command, CancellationToken cancellationToken)
    {
        var response = await Sender.Send(command, cancellationToken);

        if (response.IsFailure)
        {
            return HandleFailure(response);
        }

        return Ok(response.Value);
    }

    [HttpPost("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result);
    }

    [HttpGet("{username}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderHeaderResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> GetUserByUsername([FromRoute] string username, CancellationToken cancellationToken)
    {
        var query = new GetUserByUsernameQuery(username);
        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result);
    }
}
