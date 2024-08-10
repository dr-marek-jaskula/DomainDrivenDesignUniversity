using FluentValidation;
using Shopway.Application.Utilities;
using Shopway.Domain.Users.ValueObjects;

namespace Shopway.Application.Features.Users.Queries.GetUserRolesWithPermissions;

internal sealed class GetUserRolesWithPermissionsQueryValidator : AbstractValidator<GetUserRolesWithPermissionsQuery>
{
    public GetUserRolesWithPermissionsQueryValidator()
    {
        RuleFor(x => x.Username)
            .MustSatisfyValueObjectValidation(Username.Validate);
    }
}
