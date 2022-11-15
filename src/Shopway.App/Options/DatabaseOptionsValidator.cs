using Microsoft.Extensions.Options;
using Shopway.Domain.Extensions;

namespace Shopway.App.Options;

public sealed class DatabaseOptionsValidator : IValidateOptions<DatabaseOptions>
{
    public ValidateOptionsResult Validate(string? name, DatabaseOptions options)
    {
        var validationResult = string.Empty;

        if (options.ConnectionString.IsNullOrEmptyOrWhiteSpace())
        {
            validationResult += "Connection string is missing";
        }

        if (options.MaxRetryCount < 1)
        {
            validationResult += "Retry Count is less than one";
        }

        if (options.MaxRetryDelay < 1)
        {
            validationResult += "Retry delay is less than one";
        }

        if (options.CommandTimeout < 1)
        {
            validationResult += "Command timeout is less than one";
        }

        if (!validationResult.IsNullOrEmptyOrWhiteSpace())
        {
            return ValidateOptionsResult.Fail(validationResult);
        }

        return ValidateOptionsResult.Success;
    }
}