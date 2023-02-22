using Microsoft.Extensions.Options;
using Shopway.Domain.Utilities;
using Shopway.Infrastructure.Options;

namespace Shopway.Infrastructure.Validators;

public sealed class CacheOptionsValidator : IValidateOptions<CacheOptions>
{
    public ValidateOptionsResult Validate(string? name, CacheOptions options)
    {
        var validationResult = string.Empty;

        if (options.ConnectionString.IsNullOrEmptyOrWhiteSpace())
        {
            validationResult += "Connection string is missing. ";
        }

        if (!validationResult.IsNullOrEmptyOrWhiteSpace())
        {
            return ValidateOptionsResult.Fail(validationResult);
        }

        return ValidateOptionsResult.Success;
    }
}
