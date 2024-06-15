using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shopway.Domain.Common.Utilities;
using Shopway.Infrastructure.Options;

namespace Shopway.Infrastructure.Validators;

public sealed class OpenTelemetryOptionsValidator : IValidateOptions<OpenTelemetryOptions>
{
    public ValidateOptionsResult Validate(string? name, OpenTelemetryOptions options)
    {
        var validationResult = string.Empty;

        if (options.TeamName.IsNullOrEmptyOrWhiteSpace())
        {
            validationResult += "TeamName is missing. ";
        }

        if (options.ApplicationName.IsNullOrEmptyOrWhiteSpace())
        {
            validationResult += "Host is missing. ";
        }

        if (options.Version.IsNullOrEmptyOrWhiteSpace())
        {
            validationResult += "Version is missing. ";
        }

        if (options.OtlpCollectorHost.IsNullOrEmpty())
        {
            validationResult += "OtlpCollectorHost is missing. ";
        }

        if (options.Meters.IsNullOrEmpty())
        {
            validationResult += "Meters are null or empty. ";
        }

        if (!validationResult.IsNullOrEmptyOrWhiteSpace())
        {
            return ValidateOptionsResult.Fail(validationResult);
        }

        return ValidateOptionsResult.Success;
    }
}
