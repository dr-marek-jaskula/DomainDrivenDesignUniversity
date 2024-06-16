using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Users.Authorization;

namespace Shopway.Application.Features.Users.Commands.RemovePropertyFromReadPermission;

internal sealed class RemovePropertyFromReadPermissionCommandHandler(IAuthorizationRepository authorizationRepository, IValidator validator)
    : ICommandHandler<RemovePropertyFromReadPermissionCommand>
{
    private readonly IAuthorizationRepository _authorizationRepository = authorizationRepository;
    private readonly IValidator _validator = validator;

    public async Task<IResult> Handle(RemovePropertyFromReadPermissionCommand command, CancellationToken cancellationToken)
    {
        var permissionSuccessfulyParsed = Enum.TryParse<PermissionName>(command.Permission, out var permissionName);

        _validator
            .If(permissionSuccessfulyParsed is false, Error.NotFound(nameof(PermissionName), command.Permission, "Permissions are case sensitive."));

        if (_validator.IsInvalid)
        {
            return _validator.Failure();
        }

        var permission = await _authorizationRepository
            .GetPermissionAsync(permissionName, cancellationToken);

        _validator
            .If(permission is null, Error.NotFound(nameof(Permission), command.Permission, "Permission not found in database."))
            .If(permission?.Type is not PermissionType.Read, Error.NotFound(nameof(command.Property), command.Property, "Permission is not Read Permission."))
            .If(permission?.Properties?.Any(x => x == command.Property) is not true, Error.NotFound(nameof(command.Property), command.Property, "Property not found in Permission."));

        if (_validator.IsInvalid)
        {
            return _validator.Failure();
        }

        var permissionToRemove = permission!
            .Properties!
            .Remove(command.Property);

        return Result.Success();
    }
}
