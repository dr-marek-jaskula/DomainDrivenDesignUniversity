using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shopway.Application.Features.Users.Commands;
using Shopway.Application.Features.Users.Commands.AddPermissionToRole;
using Shopway.Application.Features.Users.Commands.LoginTwoFactorFirstStep;
using Shopway.Application.Features.Users.Commands.LoginTwoFactorSecondStep;
using Shopway.Application.Features.Users.Commands.LogUser;
using Shopway.Application.Features.Users.Commands.RefreshAccessToken;
using Shopway.Application.Features.Users.Commands.RegisterUser;
using Shopway.Application.Features.Users.Commands.RemovePermissionFromRole;
using Shopway.Application.Features.Users.Commands.Revoke;
using Shopway.Application.Features.Users.Queries.GetRolePermissions;
using Shopway.Application.Features.Users.Queries.GetUserByUsername;
using Shopway.Application.Features.Users.Queries.GetUserRoles;
using Shopway.Domain.Users;
using Shopway.Presentation.Abstractions;
using Shopway.Presentation.Authentication.RolePermissionAuthentication;
using System.Security.Claims;

namespace Shopway.Presentation.Controllers;

public sealed class UsersController(ISender sender) : ApiController(sender)
{
    [HttpPost("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<RegisterUserResponse>, ProblemHttpResult>> Register
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

        return TypedResults.Ok(result.Value);
    }

    [HttpPost("[action]")]
    [ProducesResponseType<AccessTokenResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<AccessTokenResponse>, ProblemHttpResult>> Login
    (
        [FromBody] LogUserCommand command, 
        CancellationToken cancellationToken
    )
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return TypedResults.Ok(result.Value);
    }

    [HttpPost("login/two-factor/first-step")]
    [ProducesResponseType<AccessTokenResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok, ProblemHttpResult>> LoginTwoFactorFirstStep
    (
        [FromBody] LoginTwoFactorFirstStepCommand command, 
        CancellationToken cancellationToken
    )
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return TypedResults.Ok();
    }

    [HttpPost("login/two-factor/second-step")]
    [ProducesResponseType<AccessTokenResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<AccessTokenResponse>, ProblemHttpResult>> LoginTwoFactorSecondStep
    (
        [FromBody] LoginTwoFactorSecondStepCommand command, 
        CancellationToken cancellationToken
    )
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return TypedResults.Ok(result.Value);
    }

    [HttpPost("login/two-factor/topt")]
    [ProducesResponseType<AccessTokenResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok, ProblemHttpResult>> LoginTwoFactorTopt
    (
        [FromBody] LoginTwoFactorToptCommand command,
        CancellationToken cancellationToken
    )
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return TypedResults.Ok();
    }

    [HttpPost("[action]")]
    [ProducesResponseType<AccessTokenResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<AccessTokenResponse>, ProblemHttpResult>> Refresh
    (
        [FromBody] RefreshAccessTokenCommand command, 
        CancellationToken cancellationToken
    )
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return TypedResults.Ok(result.Value);
    }

    [HttpPost("[action]")]
    [Authorize]
    [ProducesResponseType<Ok>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok, ProblemHttpResult>> Revoke(CancellationToken cancellationToken)
    {
        var parseResult = Ulid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userIdAsUlid);

        if (parseResult is false)
        {
            return TypedResults.Problem("UserId was not parsed properly");
        }

        var result = await Sender.Send(new RevokeRefreshTokenCommand(UserId.Create(userIdAsUlid)), cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return TypedResults.Ok();
    }

    [HttpGet("{username}")]
    [ProducesResponseType<UserResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<UserResponse>, ProblemHttpResult>> GetUserByUsername
    (
        [FromRoute] string username, 
        CancellationToken cancellationToken
    )
    {
        var query = new GetUserByUsernameQuery(username);
        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return TypedResults.Ok(result.Value);
    }

    [HttpGet("{username}/roles")]
    [ProducesResponseType<RolesResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<RolesResponse>, ProblemHttpResult>> GetUserRolesByUsername
    (
        [FromRoute] string username, 
        CancellationToken cancellationToken
    )
    {
        var query = new GetUserRolesByUsernameQuery(username);
        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return TypedResults.Ok(result.Value);
    }

    [HttpGet("roles/{role}/permissions")]
    [RequiredRoles(Domain.Enums.Role.Administrator)]
    [ProducesResponseType<RolesResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<RolePermissionsResponse>, ProblemHttpResult>> GetRolePermissions
    (
        [FromRoute] string role, 
        CancellationToken cancellationToken
    )
    {
        var query = new GetRolePermissionsQuery(role);
        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return TypedResults.Ok(result.Value);
    }

    [HttpPost("roles/{role}/permissions/{permission}")]
    [RequiredRoles(Domain.Enums.Role.Administrator)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok, ProblemHttpResult>> AddPermissionToRole
    (
        [FromRoute] string role, 
        [FromRoute] string permission, 
        CancellationToken cancellationToken
    )
    {
        var command = new AddPermissionToRoleCommand(role, permission);
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return TypedResults.Ok();
    }

    [HttpDelete("roles/{role}/permissions/{permission}")]
    [RequiredRoles(Domain.Enums.Role.Administrator)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok, ProblemHttpResult>> RemovePermissionFromRole
    (
        [FromRoute] string role, 
        [FromRoute] string permission, 
        CancellationToken cancellationToken
    )
    {
        var command = new RemovePermissionFromRoleCommand(role, permission);
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return TypedResults.Ok();
    }
}
