﻿using FluentValidation;
using Shopway.Application.Utilities;
using Shopway.Domain.Users.ValueObjects;

namespace Shopway.Application.Features.Users.Commands.LogUser;

internal sealed class LogUserCommandValidator : AbstractValidator<LogUserCommand>
{
    public LogUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .MustSatisfyValueObjectValidation(Email.Validate);

        RuleFor(x => x.Password)
            .MustSatisfyValueObjectValidation(Password.Validate);
    }
}
