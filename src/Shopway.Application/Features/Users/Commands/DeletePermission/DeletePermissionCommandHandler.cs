using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Users.Authorization;

namespace Shopway.Application.Features.Users.Commands.DeletePermission;

internal sealed class DeletePermissionCommandHandler(IAuthorizationRepository authorizationRepository, IValidator validator)
    : ICommandHandler<DeletePermissionCommand>
{
    private readonly IAuthorizationRepository _authorizationRepository = authorizationRepository;
    private readonly IValidator _validator = validator;

    public async Task<IResult> Handle(DeletePermissionCommand command, CancellationToken cancellationToken)
    {
        var parsePermissionNameResult = Enum.TryParse<PermissionName>(command.PermissionName, out var permissionName);

        _validator
            .If(parsePermissionNameResult is false, Error.InvalidArgument($"{command.PermissionName} is not a valid PermissionName"));

        if (_validator.IsInvalid)
        {
            return _validator.Failure();
        }

        var permission = await _authorizationRepository
            .GetPermissionAsync(permissionName, cancellationToken);

        _validator
            .If(permission is null, Error.NotFound(nameof(Permission),command.PermissionName, $"{command.PermissionName} not found in the database"));

        if (_validator.IsInvalid)
        {
            return _validator.Failure();
        }

        await _authorizationRepository
            .DeletePermissionAsync(permission!);

        return Result.Success();
    }
}
