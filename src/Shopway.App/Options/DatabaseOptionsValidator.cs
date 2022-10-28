using Microsoft.Extensions.Options;

namespace Shopway.App.Options;

//This is registered by
//1. "services.ConfigureOptions<DatabaseOptionsSetup>();"
//2. "services.AddSingleton<IValidateOptions<DatabaseOptions>, DatabaseOptionsValidator>();"
public class DatabaseOptionsValidator : IValidateOptions<DatabaseOptions>
{
    public ValidateOptionsResult Validate(string name, DatabaseOptions options)
    {
        var validationResult = string.Empty;

        if (string.IsNullOrEmpty(options.ConnectionString))
            validationResult += "Connection string is missing";

        if (options.MaxRetryCount < 1)
            validationResult += "Retry Count is less than one";

        if (options.MaxRetryDelay < 1)
            validationResult += "Retry delay is less than one";

        if (options.CommandTimeout < 1)
            validationResult += "Command timeout is less than one";

        if (!string.IsNullOrEmpty(validationResult))
            return ValidateOptionsResult.Fail(validationResult);

        return ValidateOptionsResult.Success;
    }
}