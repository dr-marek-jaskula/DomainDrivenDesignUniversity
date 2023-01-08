using MediatR;
using Shopway.Presentation.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Shopway.Presentation.Requests.Users;
using Shopway.Application.CQRS.Users.Commands.LogUser;
using Shopway.Application.CQRS.Users.Commands.CreateUser;

namespace Gatherly.Presentation.Controllers;

[Route("api/[controller]")]
public sealed class UserController : ApiController
{
    public UserController(ISender sender)
        : base(sender)
    {
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login
    (
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new LogUserCommand(request.Email, request.Password);

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
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new CreateUserCommand(request.Username, request.Email, request.Password, request.ConfirmPassword);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result);
    }
}
