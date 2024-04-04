using PaymentGateway.Cryptography;
using PaymentGateway.DummyGatewayTypes;
using PaymentGateway.HttpClients;
using Shopway.Infrastructure.Payments;

namespace PaymentGateway.Webhook;

public class WebhookService(IShopwayApi shopwayApi)
{
    private readonly List<Subscription> _subscriptions = [];

    public void Subscribe(Subscription subscription)
    {
        _subscriptions.Add(subscription);
    }

    public async Task PublishMessage(string webhookSecret, string issuer, string sessionId, string clientSecret, bool wasPaymentSuccessful)
    {
        var subscribedWebhooks = _subscriptions
            .Where(w => w.Issuer == issuer)
            .FirstOrDefault(w => w.WebhookSecret == webhookSecret);

        if (subscribedWebhooks is null)
        {
            throw new Exception("Issuer or session id not found");
        }

        var eventType = wasPaymentSuccessful
            ? Events.CheckoutSessionAsyncPaymentSucceeded
            : Events.CheckoutSessionAsyncPaymentFailed;

        var paymentResult = new PaymentGatewayEvent
        {
            Data = new Data()
            {
                Object = new Session()
                {
                    Id = sessionId,
                    ClientSecret = clientSecret,
                    Url = issuer,
                }
            },
            Type = eventType
        };

        var hashedWebhookSecret = HashUtilities.ComputeSha256Hash(webhookSecret);

        var result = await shopwayApi.SendEventToWebhook(subscribedWebhooks.Webhook, hashedWebhookSecret, paymentResult);
    }
}