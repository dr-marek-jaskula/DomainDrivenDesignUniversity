using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Users.Authorization;

namespace Shopway.Application.Features.Users.Commands.AddPropertyToReadPermission;

internal sealed class AddPropertyToReadPermissionCommandHandler(IAuthorizationRepository authorizationRepository, IValidator validator)
    : ICommandHandler<AddPropertyToReadPermissionCommand>
{
    private readonly IAuthorizationRepository _authorizationRepository = authorizationRepository;
    private readonly IValidator _validator = validator;

    public async Task<IResult> Handle(AddPropertyToReadPermissionCommand command, CancellationToken cancellationToken)
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
            .If(permission?.Properties?.Any(x => x == command.Property) is not false, Error.AlreadyExists("Property", command.Property));

        if (_validator.IsInvalid)
        {
            return _validator.Failure();
        }

        var check = IEntityUtilities.ValidateEntityProperties(permission!.RelatedEntity!, [command.Property]);

        if (check.IsFailure)
        {
            return check;
        }

        permission!
            .Properties!
            .Add(command.Property);

        return Result.Success();
    }
}
