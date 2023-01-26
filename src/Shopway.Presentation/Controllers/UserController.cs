using MediatR;
using Shopway.Presentation.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Shopway.Application.CQRS.Users.Commands.LogUser;
using Shopway.Application.CQRS.Users.Commands.CreateUser;

namespace Gatherly.Presentation.Controllers;

public sealed class UserController : ApiController
{
    public UserController(ISender sender)
        : base(sender)
    {
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login
    (
        [FromBody] LogUserCommand command,
        CancellationToken cancellationToken
    )
    {
        var response = await Sender.Send(command, cancellationToken);

        if (response.IsFailure)
        {
            return HandleFailure(response);
        }

        return Ok(response.Value);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Register
    (
        [FromBody] CreateUserCommand command,
        CancellationToken cancellationToken
    )
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result);
    }
}
