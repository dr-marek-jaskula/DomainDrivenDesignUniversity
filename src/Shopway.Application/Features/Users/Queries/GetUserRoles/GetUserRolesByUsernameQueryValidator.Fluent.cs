using FluentValidation;
using Shopway.Application.Features.Users.Queries.GetUserByUsername;

namespace Shopway.Application.Features.Users.Queries.GetUserRoles;

internal sealed class GetUserRolesByUsernameQueryValidator : AbstractValidator<GetUserByUsernameQuery>
{
    public GetUserRolesByUsernameQueryValidator()
    {
        RuleFor(x => x.Username).NotNull();
    }
}