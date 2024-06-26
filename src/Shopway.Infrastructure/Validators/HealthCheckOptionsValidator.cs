﻿using Microsoft.Extensions.Options;
using Shopway.Domain.Common.Utilities;
using Shopway.Infrastructure.Options;

namespace Shopway.Infrastructure.Validators;

public sealed class HealthOptionsValidator : IValidateOptions<HealthOptions>
{
    public ValidateOptionsResult Validate(string? name, HealthOptions options)
    {
        var validationResult = string.Empty;

        if (options.DelayInSeconds <= 1)
        {
            validationResult += "Delay must be at least two seconds. ";
        }

        if (options.PeriodInSeconds <= 1)
        {
            validationResult += "Period must be at least two seconds. ";
        }

        if (!validationResult.IsNullOrEmptyOrWhiteSpace())
        {
            return ValidateOptionsResult.Fail(validationResult);
        }

        return ValidateOptionsResult.Success;
    }
}
