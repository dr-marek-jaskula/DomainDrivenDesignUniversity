using FluentValidation;

namespace Shopway.Application.CQRS.Users.Queries.GetUserByUsername;

internal sealed class GetUserByUsernameQueryValidator : AbstractValidator<GetUserByUsernameQuery>
{
    public GetUserByUsernameQueryValidator()
    {
        RuleFor(x => x.Username).NotNull();
    }
}