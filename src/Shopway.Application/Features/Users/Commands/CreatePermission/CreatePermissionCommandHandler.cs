using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Users.Authorization;

namespace Shopway.Application.Features.Users.Commands.CreatePermission;

internal sealed class CreatePermissionCommandHandler(IAuthorizationRepository<PermissionName, RoleName> authorizationRepository, IValidator validator)
    : ICommandHandler<CreatePermissionCommand>
{
    private readonly IAuthorizationRepository<PermissionName, RoleName> _authorizationRepository = authorizationRepository;
    private readonly IValidator _validator = validator;

    public async Task<IResult> Handle(CreatePermissionCommand command, CancellationToken cancellationToken)
    {
        var permissionToCreateResult = Permission.CreatePermission(command.PermissionName, command.RelatedAggregateRoot, command.RelatedEntity, command.PermissionType, command.AllowedProperties);

        _validator
            .Validate(permissionToCreateResult);

        if (_validator.IsInvalid)
        {
            return _validator.Failure();
        }

        var permissionFromDatabase = await _authorizationRepository.GetPermissionAsync(Enum.Parse<PermissionName>(permissionToCreateResult.Value.Name), cancellationToken);


        _validator
            .If(permissionFromDatabase is not null, Error.AlreadyExists<Permission>(permissionToCreateResult.Value.Name));

        if (_validator.IsInvalid)
        {
            return _validator.Failure();
        }

        await _authorizationRepository
            .CreatePermissionAsync(permissionToCreateResult.Value);

        return Result.Success();
    }
}
