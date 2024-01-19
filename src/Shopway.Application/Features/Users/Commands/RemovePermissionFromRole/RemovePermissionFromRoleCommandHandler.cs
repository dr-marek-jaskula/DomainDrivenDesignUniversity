using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Errors;
using Shopway.Domain.Users;
using Shopway.Domain.Users.Enumerations;

namespace Shopway.Application.Features.Users.Commands.RemovePermissionFromRole;

internal sealed class RemovePermissionFromRoleCommandHandler(IUserRepository userRepository, IValidator validator)
    : ICommandHandler<RemovePermissionFromRoleCommand>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IValidator _validator = validator;

    public async Task<IResult> Handle(RemovePermissionFromRoleCommand command, CancellationToken cancellationToken)
    {
        var role = Role.FromName(command.Role);
        var permission = Permission.FromName(command.Permission);

        _validator
            .If(role is null, Error.NotFound(nameof(Role), command.Role, "Roles are case sensitive."))
            .If(permission is null, Error.NotFound(nameof(Permission), command.Permission, "Permissions are case sensitive."))
            .If(role == Role.Administrator, Error.InvalidOperation("Removing permission from to Administrator role is forbidden."));

        if (_validator.IsInvalid)
        {
            return _validator.Failure();
        }

        var roleWithPermissions = await _userRepository
            .GetRolePermissionsAsync(role!, cancellationToken);

        _validator
            .If(roleWithPermissions!.Permissions.Any(x => x.Name == permission!.Name) is false, Error.InvalidOperation("Role does not contain given permission."));

        if (_validator.IsInvalid)
        {
            return _validator.Failure();
        }

        var permissionToRemove = roleWithPermissions
            .Permissions
            .First(x => x.Name == permission!.Name);

        roleWithPermissions!
            .Permissions
            .Remove(permissionToRemove);

        return Result.Success();
    }
}