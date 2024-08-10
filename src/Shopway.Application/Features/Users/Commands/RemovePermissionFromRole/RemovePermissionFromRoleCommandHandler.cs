using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Users.Authorization;

namespace Shopway.Application.Features.Users.Commands.RemovePermissionFromRole;

internal sealed class RemovePermissionFromRoleCommandHandler(IAuthorizationRepository<PermissionName, RoleName> authorizationRepository, IValidator validator)
    : ICommandHandler<RemovePermissionFromRoleCommand>
{
    private readonly IAuthorizationRepository<PermissionName, RoleName> _authorizationRepository = authorizationRepository;
    private readonly IValidator _validator = validator;

    public async Task<IResult> Handle(RemovePermissionFromRoleCommand command, CancellationToken cancellationToken)
    {
        var permissionSuccessfulyParsed = Enum.TryParse<PermissionName>(command.Permission, out var permissionName);
        var roleSuccessfulyParsed = Enum.TryParse<RoleName>(command.Role, out var roleName);

        _validator
            .If(roleSuccessfulyParsed is false, Error.NotFound(nameof(RoleName), command.Role, "Roles are case sensitive."))
            .If(permissionSuccessfulyParsed is false, Error.NotFound(nameof(PermissionName), command.Permission, "Permissions are case sensitive."))
            .If(roleName is RoleName.Administrator, Error.InvalidOperation("Removing permission from to Administrator role is forbidden."));

        if (_validator.IsInvalid)
        {
            return _validator.Failure();
        }

        var roleWithPermissions = await _authorizationRepository
            .GetRolePermissionsAsync(roleName, cancellationToken);

        _validator
            .If(roleWithPermissions is null, Error.NotFound(nameof(Role), command.Role, "Role not found in database."))
            .If(roleWithPermissions?.Permissions.Any(x => x.Name == $"{permissionName}") is not true, Error.InvalidOperation("Role does not contain given permission."));

        if (_validator.IsInvalid)
        {
            return _validator.Failure();
        }

        var permissionToRemove = roleWithPermissions!
            .Permissions
            .First(x => x.Name == $"{permissionName}");

        roleWithPermissions!
            .Permissions
            .Remove(permissionToRemove);

        return Result.Success();
    }
}
