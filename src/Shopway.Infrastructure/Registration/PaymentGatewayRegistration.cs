using Microsoft.Extensions.Options;
using Shopway.Domain.Orders;
using Shopway.Infrastructure.Options;
using Shopway.Infrastructure.Payments;
using Shopway.Infrastructure.Payments.DummyGatewayTypes;

namespace Microsoft.Extensions.DependencyInjection;

public static class PaymentGatewayRegistration
{
    internal static IServiceCollection RegisterPaymentGateway(this IServiceCollection services)
    {
        //Services
        services.AddScoped<IPaymentGatewayService, PaymentGatewayService>();
        services.ConfigureOptions<PaymentGatewayOptionsSetup>();
        services.AddSingleton<IValidateOptions<PaymentGatewayOptions>, PaymentGatewayOptionsValidator>();
        
        SetPaymentGatewayApiKey(services);

        return services;
    }

    private static void SetPaymentGatewayApiKey(IServiceCollection services)
    {
        var options = services.GetOptions<PaymentGatewayOptions>();
        PaymentGatewayConfiguration.ApiKey = options.SecretKey!;
    }
}
