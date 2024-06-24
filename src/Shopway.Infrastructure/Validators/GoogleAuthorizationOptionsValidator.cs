using Microsoft.Extensions.Options;
using Shopway.Domain.Common.Utilities;
using Shopway.Infrastructure.Options;

namespace Shopway.Infrastructure.Validators;

public sealed class GoogleAuthorizationOptionsValidator : IValidateOptions<GoogleAuthorizationOptions>
{
    public ValidateOptionsResult Validate(string? name, GoogleAuthorizationOptions options)
    {
        var validationResult = string.Empty;

        if (options.ClientId.IsNullOrEmptyOrWhiteSpace())
        {
            validationResult += "ClientId is missing. ";
        }

        if (options.ClientSecret.IsNullOrEmptyOrWhiteSpace())
        {
            validationResult += "ClientSecret is missing. ";
        }

        if (!validationResult.IsNullOrEmptyOrWhiteSpace())
        {
            return ValidateOptionsResult.Fail(validationResult);
        }

        return ValidateOptionsResult.Success;
    }
}
