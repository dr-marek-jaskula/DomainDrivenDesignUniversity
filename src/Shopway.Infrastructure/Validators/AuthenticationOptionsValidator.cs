using Microsoft.Extensions.Options;
using Shopway.Domain.Common.Utilities;
using Shopway.Infrastructure.Options;

namespace Shopway.Infrastructure.Validators;

public sealed class AuthenticationOptionsValidator : IValidateOptions<AuthenticationOptions>
{
    public ValidateOptionsResult Validate(string? name, AuthenticationOptions options)
    {
        var validationResult = string.Empty;

        if (options.AccessTokenExpirationInMinutes <= 0)
        {
            validationResult += $"Invalid {nameof(options.AccessTokenExpirationInMinutes)}. ";
        }

        if (options.RefreshTokenExpirationInDays <= 0)
        {
            validationResult += $"Invalid {nameof(options.RefreshTokenExpirationInDays)}. ";
        }

        if (options.SecretKey.Length < 5)
        {
            validationResult += $"To short {nameof(options.SecretKey)}. ";
        }

        if (!validationResult.IsNullOrEmptyOrWhiteSpace())
        {
            return ValidateOptionsResult.Fail(validationResult);
        }

        return ValidateOptionsResult.Success;
    }
}