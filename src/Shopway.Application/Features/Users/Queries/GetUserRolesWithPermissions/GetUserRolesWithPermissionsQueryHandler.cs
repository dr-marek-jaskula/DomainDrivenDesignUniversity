using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Users;
using Shopway.Domain.Users.ValueObjects;

namespace Shopway.Application.Features.Users.Queries.GetUserRolesWithPermissions;

internal sealed class GetUserRolesWithPermissionsQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetUserRolesWithPermissionsQuery, RolesWithPermissionsResponse>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<IResult<RolesWithPermissionsResponse>> Handle(GetUserRolesWithPermissionsQuery query, CancellationToken cancellationToken)
    {
        var username = Username.Create(query.Username).Value;

        var user = await _userRepository
            .GetByUsernameAsync(username, cancellationToken);

        if (user is null)
        {
            return Error.NotFound<User>(query.Username).ToResult<RolesWithPermissionsResponse>();
        }

        return user
            .Roles
            .ToRolesWithPermissionsResponse()
            .ToResult();
    }
}
