using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Abstractions;
using Shopway.Application.Features.Users.Commands.AddRoleToUser;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Users.Authorization;
using Shopway.Domain.Users.ValueObjects;
using Shopway.Domain.Users;
using Shopway.Domain.Common.Errors;

namespace Shopway.Application.Features.Users.Commands.RemoveRoleFromUser;

internal sealed class RemoveRoleFromUserCommandHandler(IUserRepository userRepository, IAuthorizationRepository authorizationRepository, IValidator validator, IUserContextService userContextService)
    : ICommandHandler<RemoveRoleFromUserCommand>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAuthorizationRepository _authorizationRepository = authorizationRepository;
    private readonly IValidator _validator = validator;
    private readonly IUserContextService _userContextService = userContextService;

    public async Task<IResult> Handle(RemoveRoleFromUserCommand command, CancellationToken cancellationToken)
    {
        var username = Username.Create(command.Username);
        var roleSuccessfulyParsed = Enum.TryParse<RoleName>(command.Role, out var roleName);

        _validator
            .Validate(username)
            .If(roleSuccessfulyParsed is false, Error.NotFound(nameof(RoleName), command.Role, "Roles are case sensitive."));

        if (_validator.IsInvalid)
        {
            return _validator.Failure();
        }

        var user = await _userRepository
            .GetByUsernameAsync(username.Value, cancellationToken);

        _validator
            .If(user is null, Error.NotFound<User>(command.Username));

        if (_validator.IsInvalid)
        {
            return _validator.Failure();
        }

        var role = await _authorizationRepository
            .GetRolePermissionsAsync(roleName, cancellationToken);

        _validator
            .If(role is null, Error.NotFound(nameof(Role), $"{roleName}", "RoleName does not match role in database"))
            .If(user!.Roles.Any(x => x.Name == role?.Name) is not true, Error.NotFound(nameof(Role), $"{roleName}", "User does not have such role"));

        if (_validator.IsInvalid)
        {
            return _validator.Failure();
        }

        user!.RemoveRole(role!);

        return Result.Success();
    }
}
