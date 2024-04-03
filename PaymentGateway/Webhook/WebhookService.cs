using Microsoft.AspNetCore.Mvc.Formatters;
using PaymentGateway.Cryptography;
using PaymentGateway.DummyGatewayTypes;
using Shopway.Infrastructure.Payments;
using System.Text;
using System.Text.Json;

namespace PaymentGateway.Webhook;

public class WebhookService(IServiceScopeFactory serviceScopeFactory)
{
    private readonly List<Subscription> _subscriptions = [];
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

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

        using var createStope = _serviceScopeFactory.CreateScope();

        var httpClientFactory = createStope.ServiceProvider.GetRequiredService<IHttpClientFactory>();

        using var httpClient = httpClientFactory.CreateClient("with-api-version");

        StringContent httpContent = GetRequestBody(paymentResult);
        SignRequest(hashedWebhookSecret, httpClient);

        var result = await httpClient.PostAsync(subscribedWebhooks.Webhook, httpContent);
    }

    private static StringContent GetRequestBody(PaymentGatewayEvent paymentResult)
    {
        var json = JsonSerializer.Serialize(paymentResult);
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
        return httpContent;
    }

    private static void SignRequest(string hashedWebhookSecret, HttpClient httpClient)
    {
        const string SignatureHeader = "PaymentGateway-Signature";
        httpClient.DefaultRequestHeaders.Add(SignatureHeader, hashedWebhookSecret);
    }
}