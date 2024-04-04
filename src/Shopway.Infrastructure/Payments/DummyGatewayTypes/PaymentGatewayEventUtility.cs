using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Shopway.Domain.Common.Utilities;
using Shopway.Infrastructure.Payments.DummyGatewayTypes;

namespace Shopway.Infrastructure.Payments;

public static class PaymentGatewayEventUtility
{
    internal static PaymentGatewayEvent ConstructEvent(string json, StringValues paymentGatewaySignature, string webhookSecret)
    {
        //Compute hashes and compare with signature (payment gateways do similar things)
        var hashedSecret = HashUtilities.ComputeSha256Hash(webhookSecret);

        if (paymentGatewaySignature != hashedSecret)
        {
            throw new PaymentGatewayException("Invalid Client Secret");
        }

        return JsonConvert.DeserializeObject<PaymentGatewayEvent>(json)
            ?? throw new PaymentGatewayException("PaymentGatewayEvent has invalid format");
    }
}
