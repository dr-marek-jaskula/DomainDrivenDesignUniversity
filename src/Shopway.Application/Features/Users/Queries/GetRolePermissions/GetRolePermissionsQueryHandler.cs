using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Users.Authorization;

namespace Shopway.Application.Features.Users.Queries.GetRolePermissions;

internal sealed class GetRolePermissionsQueryHandler(IAuthorizationRepository<PermissionName, RoleName> authorizationRepository, IValidator validator)
    : IQueryHandler<GetRolePermissionsQuery, RolePermissionsResponse>
{
    private readonly IAuthorizationRepository<PermissionName, RoleName> _authorizationRepository = authorizationRepository;
    private readonly IValidator _validator = validator;

    public async Task<IResult<RolePermissionsResponse>> Handle(GetRolePermissionsQuery query, CancellationToken cancellationToken)
    {
        var successfulyParsed = Enum.TryParse<RoleName>(query.Role, out var roleName);

        _validator
            .If(successfulyParsed is false, Error.NotFound(nameof(RoleName), query.Role, "Roles are case sensitive."));

        if (_validator.IsInvalid)
        {
            return _validator.Failure<RolePermissionsResponse>();
        }

        var roleWithPermissions = await _authorizationRepository
            .GetRolePermissionsAsync(roleName, cancellationToken);

        _validator
            .If(roleWithPermissions is null, Error.NotFound(nameof(Role), query.Role, "Role not found in database."));

        if (_validator.IsInvalid)
        {
            return _validator.Failure<RolePermissionsResponse>();
        }

        return roleWithPermissions!
            .ToResponse()
            .ToResult();
    }
}
