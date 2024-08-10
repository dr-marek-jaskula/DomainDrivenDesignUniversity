using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Users.Authorization;

namespace Shopway.Application.Features.Users.Commands.DeletePermission;

internal sealed class DeletePermissionCommandHandler(IAuthorizationRepository<PermissionName, RoleName> authorizationRepository)
    : ICommandHandler<DeletePermissionCommand>
{
    private readonly IAuthorizationRepository<PermissionName, RoleName> _authorizationRepository = authorizationRepository;

    public async Task<IResult> Handle(DeletePermissionCommand command, CancellationToken cancellationToken)
    {
        var permissionName = Enum.Parse<PermissionName>(command.PermissionName);

        var permission = await _authorizationRepository
            .GetPermissionAsync(permissionName, cancellationToken);

        if (permission is null)
        {
            return Error.NotFound(nameof(Permission), command.PermissionName, $"{command.PermissionName} not found in the database")
                .ToResult();
        }

        await _authorizationRepository
            .DeletePermissionAsync(permission!);

        return Result.Success();
    }
}
