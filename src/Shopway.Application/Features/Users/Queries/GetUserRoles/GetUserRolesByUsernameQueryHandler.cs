using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Users;
using Shopway.Domain.Users.ValueObjects;

namespace Shopway.Application.Features.Users.Queries.GetUserRoles;

internal sealed class GetUserRolesByUsernameQueryHandler(IUserRepository userRepository, IValidator validator)
    : IQueryHandler<GetUserRolesByUsernameQuery, RolesResponse>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IValidator _validator = validator;

    public async Task<IResult<RolesResponse>> Handle(GetUserRolesByUsernameQuery query, CancellationToken cancellationToken)
    {
        var usernameResult = Username.Create(query.Username);

        _validator
            .Validate(usernameResult);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<RolesResponse>();
        }

        var user = await _userRepository
            .GetByUsernameAsync(usernameResult.Value, cancellationToken);

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
