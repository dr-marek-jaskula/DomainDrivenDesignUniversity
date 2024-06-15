using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Shopway.Infrastructure.Payments;

internal sealed class PaymentGatewayOptionsSetup(IConfiguration configuration) : IConfigureOptions<PaymentGatewayOptions>
{
    private const string _configurationSectionName = "PaymentGatewayOptions";
    private readonly IConfiguration _configuration = configuration;

    public void Configure(PaymentGatewayOptions options)
    {
        _configuration
            .GetSection(_configurationSectionName)
            .Bind(options);
    }
}
