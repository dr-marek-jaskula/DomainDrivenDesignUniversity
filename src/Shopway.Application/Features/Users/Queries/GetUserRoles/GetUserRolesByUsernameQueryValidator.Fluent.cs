using FluentValidation;
using Shopway.Application.Features.Users.Queries.GetUserByUsername;
using Shopway.Application.Utilities;
using Shopway.Domain.Users.ValueObjects;

namespace Shopway.Application.Features.Users.Queries.GetUserRoles;

internal sealed class GetUserRolesByUsernameQueryValidator : AbstractValidator<GetUserByUsernameQuery>
{
    public GetUserRolesByUsernameQueryValidator()
    {
        RuleFor(x => x.Username)
            .MustSatisfyValueObjectValidation(Username.Validate);
    }
}
