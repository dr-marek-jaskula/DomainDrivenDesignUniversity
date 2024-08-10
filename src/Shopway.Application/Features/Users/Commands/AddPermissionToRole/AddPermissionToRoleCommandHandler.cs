using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Features.Users.Queries.GetPermissionDetails;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Users.Authorization;

namespace Shopway.Application.Features.Users.Commands.AddPermissionToRole;

internal sealed class AddPermissionToRoleCommandHandler(IAuthorizationRepository<PermissionName, RoleName> authorizationRepository, IValidator validator)
    : ICommandHandler<AddPermissionToRoleCommand>
{
    private readonly IAuthorizationRepository<PermissionName, RoleName> _authorizationRepository = authorizationRepository;
    private readonly IValidator _validator = validator;

    public async Task<IResult> Handle(AddPermissionToRoleCommand command, CancellationToken cancellationToken)
    {
        var permissionSuccessfulyParsed = Enum.TryParse<PermissionName>(command.Permission, out var permissionName);
        var roleSuccessfulyParsed = Enum.TryParse<RoleName>(command.Role, out var roleName);

        _validator
            .If(roleSuccessfulyParsed is false, Error.NotFound(nameof(RoleName), command.Role, "Roles are case sensitive."))
            .If(permissionSuccessfulyParsed is false, Error.NotFound(nameof(PermissionName), command.Permission, "Permissions are case sensitive."))
            .If(roleName is RoleName.Administrator, Error.InvalidOperation("Adding permission to Administrator role is forbidden."));

        if (_validator.IsInvalid)
        {
            return _validator.Failure();
        }

        var roleWithPermissions = await _authorizationRepository
            .GetRolePermissionsAsync(roleName, cancellationToken);

        _validator
            .If(roleWithPermissions is null, Error.NotFound(nameof(Role), command.Role, "Role not found in database."))
            .If(roleWithPermissions?.Permissions.Any(x => x.Name == $"{permissionName}") is not false, Error.AlreadyExists(nameof(Permission), command.Permission));

        if (_validator.IsInvalid)
        {
            return _validator.Failure();
        }

        var permission = await _authorizationRepository
            .GetPermissionAsync(permissionName, cancellationToken);

        _validator
            .If(permission is null, Error.NotFound(nameof(Permission), command.Permission, "Permission not found in database."));

        if (_validator.IsInvalid)
        {
            return _validator.Failure<PermissionDetailsResponse>();
        }

        roleWithPermissions!
            .Permissions
            .Add(permission!);

        return Result.Success();
    }
}
