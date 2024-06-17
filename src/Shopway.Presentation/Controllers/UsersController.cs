using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shopway.Application.Features.Users.Commands;
using Shopway.Application.Features.Users.Commands.AddPermissionToRole;
using Shopway.Application.Features.Users.Commands.AddPropertyToReadPermission;
using Shopway.Application.Features.Users.Commands.AddRoleToUser;
using Shopway.Application.Features.Users.Commands.ConfigureTwoFactorToptLogin;
using Shopway.Application.Features.Users.Commands.CreatePermission;
using Shopway.Application.Features.Users.Commands.DeletePermission;
using Shopway.Application.Features.Users.Commands.LoginTwoFactorFirstStep;
using Shopway.Application.Features.Users.Commands.LoginTwoFactorSecondStep;
using Shopway.Application.Features.Users.Commands.LoginTwoFactorTopt;
using Shopway.Application.Features.Users.Commands.LogUser;
using Shopway.Application.Features.Users.Commands.RefreshAccessToken;
using Shopway.Application.Features.Users.Commands.RegisterUser;
using Shopway.Application.Features.Users.Commands.RemovePermissionFromRole;
using Shopway.Application.Features.Users.Commands.RemovePropertyFromReadPermission;
using Shopway.Application.Features.Users.Commands.RemoveRoleFromUser;
using Shopway.Application.Features.Users.Commands.Revoke;
using Shopway.Application.Features.Users.Queries.GetPermissionDetails;
using Shopway.Application.Features.Users.Queries.GetRolePermissions;
using Shopway.Application.Features.Users.Queries.GetUserByUsername;
using Shopway.Application.Features.Users.Queries.GetUserRoles;
using Shopway.Domain.Users;
using Shopway.Domain.Users.Authorization;
using Shopway.Presentation.Abstractions;
using Shopway.Presentation.Authentication.RolePermissionAuthentication;
using Shopway.Presentation.Utilities;
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

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
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

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
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

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
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

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    [HttpPost("configure/two-factor/topt")]
    [Authorize]
    [ProducesResponseType<TwoFactorToptResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<TwoFactorToptResponse>, ProblemHttpResult>> ConfigureTwoFactorTopt(CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new ConfigureTwoFactorToptLoginCommand(), cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    [HttpPost("login/two-factor/topt")]
    [ProducesResponseType<AccessTokenResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<AccessTokenResponse>, ProblemHttpResult>> LoginTwoFactorTopt
    (
        [FromBody] LoginTwoFactorToptCommand command,
        CancellationToken cancellationToken
    )
    {
        var result = await Sender.Send(command, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
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

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
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

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
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

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
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

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    [HttpPost("{username}/roles/{role}")]
    [RequiredRoles(RoleName.Administrator)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok, ProblemHttpResult>> AddRoleToUser
    (
        [FromRoute] string username,
        [FromRoute] string role,
        CancellationToken cancellationToken
    )
    {
        var command = new AddRoleToUserCommand(username, role);
        var result = await Sender.Send(command, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    [HttpDelete("{username}/roles/{role}")]
    [RequiredRoles(RoleName.Administrator)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok, ProblemHttpResult>> RemoveRoleFromUser
    (
        [FromRoute] string username,
        [FromRoute] string role,
        CancellationToken cancellationToken
    )
    {
        var command = new RemoveRoleFromUserCommand(username, role);
        var result = await Sender.Send(command, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    [HttpGet("roles/{role}/permissions")]
    [RequiredRoles(RoleName.Administrator)]
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

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    [HttpGet("permissions/{permission}")]
    [RequiredRoles(RoleName.Administrator)]
    [ProducesResponseType<RolesResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<PermissionDetailsResponse>, ProblemHttpResult>> GetPermissionDetails
    (
        [FromRoute] string permission,
        CancellationToken cancellationToken
    )
    {
        var query = new GetPermissionDetailsQuery(permission);
        var result = await Sender.Send(query, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    [HttpPost("permissions")]
    [RequiredRoles(RoleName.Administrator)]
    [ProducesResponseType<RolesResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok, ProblemHttpResult>> CreatePermission
    (
        [FromBody] CreatePermissionCommand command,
        CancellationToken cancellationToken
    )
    {
        var result = await Sender.Send(command, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    [HttpDelete("permissions/{permission}")]
    [RequiredRoles(RoleName.Administrator)]
    [ProducesResponseType<RolesResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok, ProblemHttpResult>> DeletePermission
    (
        [FromRoute] string permission,
        CancellationToken cancellationToken
    )
    {
        var command = new DeletePermissionCommand(permission);
        var result = await Sender.Send(command, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    [HttpPost("roles/{role}/permissions/{permission}")]
    [RequiredRoles(RoleName.Administrator)]
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

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    [HttpDelete("roles/{role}/permissions/{permission}")]
    [RequiredRoles(RoleName.Administrator)]
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

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    [HttpPost("permissions/{permission}/properties/{property}")]
    [RequiredRoles(RoleName.Administrator)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok, ProblemHttpResult>> AddPropertyToReadPermission
    (
        [FromRoute] string permission,
        [FromRoute] string property,
        CancellationToken cancellationToken
    )
    {
        var command = new AddPropertyToReadPermissionCommand(permission, property);
        var result = await Sender.Send(command, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }

    [HttpDelete("permissions/{permission}/properties/{property}")]
    [RequiredRoles(RoleName.Administrator)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok, ProblemHttpResult>> RemovePropertyToReadPermission
    (
        [FromRoute] string permission,
        [FromRoute] string property,
        CancellationToken cancellationToken
    )
    {
        var command = new RemovePropertyFromReadPermissionCommand(permission, property);
        var result = await Sender.Send(command, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }
}
