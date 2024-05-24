using FluentValidation;
using Shopway.Application.Utilities;
using Shopway.Domain.Users.ValueObjects;

namespace Shopway.Application.Features.Users.Commands.LoginTwoFactorTopt;

internal sealed class LoginTwoFactorToptCommandValidator : AbstractValidator<LoginTwoFactorToptCommand>
{
    public LoginTwoFactorToptCommandValidator()
    {
        RuleFor(x => x.Email)
            .MustSatisfyValueObjectValidation(Email.Validate);

        RuleFor(x => x.Password)
            .MustSatisfyValueObjectValidation(Password.Validate);
    }
}
