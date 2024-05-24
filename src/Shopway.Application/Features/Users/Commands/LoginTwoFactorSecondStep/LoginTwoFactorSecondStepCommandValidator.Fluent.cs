using FluentValidation;
using Shopway.Application.Utilities;
using Shopway.Domain.Users.ValueObjects;

namespace Shopway.Application.Features.Users.Commands.LoginTwoFactorSecondStep;

internal sealed class LoginTwoFactorSecondStepCommandValidator : AbstractValidator<LoginTwoFactorSecondStepCommand>
{
    public LoginTwoFactorSecondStepCommandValidator()
    {
        RuleFor(x => x.Email)
            .MustSatisfyValueObjectValidation(Email.Validate);

        RuleFor(x => x.Password)
            .MustSatisfyValueObjectValidation(Password.Validate);

        RuleFor(x => x.TwoFactorToken)
            .MustSatisfyValueObjectValidation(TwoFactorTokenHash.Validate);
    }
}
