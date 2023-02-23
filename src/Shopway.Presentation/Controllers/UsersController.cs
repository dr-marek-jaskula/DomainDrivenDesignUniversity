using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopway.Presentation.Abstractions;
using Shopway.Application.CQRS.Users.Commands.LogUser;
using Shopway.Application.CQRS.Users.Commands.RegisterUser;

namespace Gatherly.Presentation.Controllers;

public sealed class UsersController : ApiController
{
    public UsersController(ISender sender)
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
        [FromBody] RegisterUserCommand command,
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
