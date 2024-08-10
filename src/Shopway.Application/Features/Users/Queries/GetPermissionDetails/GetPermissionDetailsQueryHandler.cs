using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Users.Authorization;

namespace Shopway.Application.Features.Users.Queries.GetPermissionDetails;

internal sealed class GetPermissionDetailsQueryHandler(IAuthorizationRepository<PermissionName, RoleName> authorizationRepository, IValidator validator)
    : IQueryHandler<GetPermissionDetailsQuery, PermissionDetailsResponse>
{
    private readonly IAuthorizationRepository<PermissionName, RoleName> _authorizationRepository = authorizationRepository;
    private readonly IValidator _validator = validator;

    public async Task<IResult<PermissionDetailsResponse>> Handle(GetPermissionDetailsQuery query, CancellationToken cancellationToken)
    {
        var successfulyParsed = Enum.TryParse<PermissionName>(query.Permission, out var permissionName);

        _validator
            .If(successfulyParsed is false, Error.NotFound(nameof(PermissionName), query.Permission, "Permissions are case sensitive."));

        if (_validator.IsInvalid)
        {
            return _validator.Failure<PermissionDetailsResponse>();
        }

        var permission = await _authorizationRepository
            .GetPermissionAsync(permissionName, cancellationToken);

        _validator
            .If(permission is null, Error.NotFound(nameof(Permission), query.Permission, "Permission not found in database."));

        if (_validator.IsInvalid)
        {
            return _validator.Failure<PermissionDetailsResponse>();
        }

        return permission!
            .ToDetailedResponse()
            .ToResult();
    }
}
