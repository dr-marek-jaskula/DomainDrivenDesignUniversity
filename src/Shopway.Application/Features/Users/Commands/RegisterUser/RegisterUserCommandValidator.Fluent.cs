using FluentValidation;
using Shopway.Application.Utilities;
using Shopway.Domain.Users.ValueObjects;

namespace Shopway.Application.Features.Users.Commands.RegisterUser;

internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password)
            .WithMessage("{PropertyName} failed");

        RuleFor(x => x.Email)
            .MustSatisfyValueObjectValidation(Email.Validate);

        RuleFor(x => x.Username)
            .MustSatisfyValueObjectValidation(Username.Validate);

        RuleFor(x => x.Password)
            .MustSatisfyValueObjectValidation(Password.Validate);
    }
}
