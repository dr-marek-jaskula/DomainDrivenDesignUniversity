using PaymentGateway.DummyGatewayTypes;
using Refit;

namespace PaymentGateway.HttpClients;

public interface IShopwayApi
{
    private const string SignatureHeader = "PaymentGateway-Signature";

    [Post("/{webhook}")]
    [QueryUriFormat(UriFormat.Unescaped)]
    Task<HttpResponseMessage> SendEventToWebhook(string webhook, [Header(SignatureHeader)] string signatureHeader, [Body] PaymentGatewayEvent paymentGatewayEvent);
}
