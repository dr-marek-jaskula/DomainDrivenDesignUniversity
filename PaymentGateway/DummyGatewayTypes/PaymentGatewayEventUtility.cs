using Microsoft.Extensions.Primitives;
using PaymentGateway.Cryptography;
using System.Text.Json;

namespace PaymentGateway.DummyGatewayTypes;

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

        return JsonSerializer.Deserialize<PaymentGatewayEvent>(json) 
            ?? throw new PaymentGatewayException("PaymentGatewayEvent has invalid format");
    }
}
