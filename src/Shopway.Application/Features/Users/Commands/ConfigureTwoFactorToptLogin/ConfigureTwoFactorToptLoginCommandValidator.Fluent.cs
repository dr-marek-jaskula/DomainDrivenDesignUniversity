using FluentValidation;
using Shopway.Application.Abstractions;
using Shopway.Domain.Users.ValueObjects;
using static Shopway.Application.Utilities.FluentValidationUtilities;

namespace Shopway.Application.Features.Users.Commands.ConfigureTwoFactorToptLogin;

internal sealed class ConfigureTwoFactorToptLoginCommandValidator : AbstractValidator<ConfigureTwoFactorToptLoginCommand>
{
    public ConfigureTwoFactorToptLoginCommandValidator(IUserContextService userContextService)
    {
        var username = userContextService.Username ?? string.Empty;

        RuleFor(x => x)
            .MustSatisfyValueObjectValidation(() => Username.Validate(username))
            .OverridePropertyName(nameof(Username));
    }
}
