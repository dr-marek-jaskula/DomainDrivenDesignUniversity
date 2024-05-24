using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Users;
using Shopway.Domain.Users.ValueObjects;

namespace Shopway.Application.Features.Users.Queries.GetUserByUsername;

internal sealed class GetUserByUsernameQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetUserByUsernameQuery, UserResponse>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<IResult<UserResponse>> Handle(GetUserByUsernameQuery query, CancellationToken cancellationToken)
    {
        var username = Username.Create(query.Username).Value;

        var user = await _userRepository
            .GetByUsernameAsync(username, cancellationToken);

        if (user is null)
        {
            return Result.Failure<UserResponse>(Error.NotFound<User>(query.Username));
        }

        return user
            .ToResponse()
            .ToResult();
    }
}
