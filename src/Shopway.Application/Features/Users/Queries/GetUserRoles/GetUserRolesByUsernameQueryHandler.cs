using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Users;
using Shopway.Domain.Users.ValueObjects;

namespace Shopway.Application.Features.Users.Queries.GetUserRoles;

internal sealed class GetUserRolesByUsernameQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetUserRolesByUsernameQuery, RolesResponse>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<IResult<RolesResponse>> Handle(GetUserRolesByUsernameQuery query, CancellationToken cancellationToken)
    {
        var username = Username.Create(query.Username).Value;

        var user = await _userRepository
            .GetByUsernameAsync(username, cancellationToken);

        if (user is null)
        {
            return Result.Failure<RolesResponse>(Error.NotFound<User>(query.Username));
        }

        return user
            .Roles
            .ToResponse()
            .ToResult();
    }
}
