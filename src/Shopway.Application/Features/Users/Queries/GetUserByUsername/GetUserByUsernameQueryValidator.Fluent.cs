using FluentValidation;
using Shopway.Application.Utilities;
using Shopway.Domain.Users.ValueObjects;

namespace Shopway.Application.Features.Users.Queries.GetUserByUsername;

internal sealed class GetUserByUsernameQueryValidator : AbstractValidator<GetUserByUsernameQuery>
{
    public GetUserByUsernameQueryValidator()
    {
        RuleFor(x => x.Username)
            .MustSatisfyValueObjectValidation(Username.Validate);
    }
}
