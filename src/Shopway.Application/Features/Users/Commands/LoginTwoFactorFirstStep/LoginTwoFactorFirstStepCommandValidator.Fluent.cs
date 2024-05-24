using FluentValidation;
using Shopway.Application.Utilities;
using Shopway.Domain.Users.ValueObjects;

namespace Shopway.Application.Features.Users.Commands.LoginTwoFactorFirstStep;

internal sealed class LoginTwoFactorFirstStepCommandValidator : AbstractValidator<LoginTwoFactorFirstStepCommand>
{
    public LoginTwoFactorFirstStepCommandValidator()
    {
        RuleFor(x => x.Email)
            .MustSatisfyValueObjectValidation(Email.Validate);

        RuleFor(x => x.Password)
            .MustSatisfyValueObjectValidation(Password.Validate);
    }
}
