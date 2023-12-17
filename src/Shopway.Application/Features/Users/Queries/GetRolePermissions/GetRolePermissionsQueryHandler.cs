using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Users;
using Shopway.Application.Mappings;
using Shopway.Domain.Users.Enumerations;
using Shopway.Application.Abstractions;
using Shopway.Domain.Errors;
using Shopway.Application.Utilities;

namespace Shopway.Application.Features.Users.Queries.GetRolePermissions;

internal sealed class GetRolePermissionsQueryHandler(IUserRepository userRepository, IValidator validator)
    : IQueryHandler<GetRolePermissionsQuery, RolePermissionsResponse>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IValidator _validator = validator;

    public async Task<IResult<RolePermissionsResponse>> Handle(GetRolePermissionsQuery query, CancellationToken cancellationToken)
    {
        var role = Role.FromName(query.Role);

        _validator
            .If(role is null, Error.NotFound(nameof(Role), query.Role, "Roles are case sensitive."));

        if (_validator.IsInvalid)
        {
            return _validator.Failure<RolePermissionsResponse>();
        }

        var roleWithPermissions = await _userRepository
            .GetRolePermissionsAsync(role!, cancellationToken);

        if (roleWithPermissions is null)
        {
            return Result.Failure<RolePermissionsResponse>(Error.NotFound<User>(query.Role));
        }

        return roleWithPermissions
            .ToResponse()
            .ToResult();
    }
}