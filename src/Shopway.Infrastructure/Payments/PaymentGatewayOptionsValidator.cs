using Microsoft.Extensions.Options;
using Shopway.Domain.Common.Utilities;

namespace Shopway.Infrastructure.Payments;

public sealed class PaymentGatewayOptionsValidator : IValidateOptions<PaymentGatewayOptions>
{
    public ValidateOptionsResult Validate(string? name, PaymentGatewayOptions options)
    {
        var validationResult = string.Empty;

        if (options.SecretKey.IsNullOrEmptyOrWhiteSpace())
        {
            validationResult += $"Invalid {nameof(options.SecretKey)}. ";
        }

        if (options.PublicKey.IsNullOrEmptyOrWhiteSpace())
        {
            validationResult += $"Invalid {nameof(options.PublicKey)}. ";
        }

        if (options.WebhookSecret.IsNullOrEmptyOrWhiteSpace())
        {
            validationResult += $"To short {nameof(options.WebhookSecret)}. ";
        }

        if (!validationResult.IsNullOrEmptyOrWhiteSpace())
        {
            return ValidateOptionsResult.Fail(validationResult);
        }

        return ValidateOptionsResult.Success;
    }
}
