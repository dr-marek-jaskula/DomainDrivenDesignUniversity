namespace PaymentGateway.Webhook;

public class WebhookService(IServiceScopeFactory serviceScopeFactory)
{
    private readonly List<Subscription> _subscriptions = [];
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    public void Subscribe(Subscription subscription)
    {
        _subscriptions.Add(subscription);
    }

    public async Task PublishMessage(string secretHash, string issuer, string sessionId, bool wasPaymentSuccessful)
    {
        var subscribedWebhooks = _subscriptions
            .Where(w => w.Issuer == issuer)
            .FirstOrDefault(w => w.SessionId == sessionId);

        if (subscribedWebhooks is null)
        {
            throw new Exception("Issuer or session id not found");
        }

        var paymentResult = new
        {
            SessionId = sessionId,
            WasPaymentSuccessful = wasPaymentSuccessful,
            SecretHash = secretHash
        };

        using var createStope = _serviceScopeFactory.CreateScope();

        var httpClientFactory = createStope.ServiceProvider.GetRequiredService<IHttpClientFactory>();
        using var httpClient = httpClientFactory.CreateClient("with-api-version");

        var result = await httpClient.PostAsJsonAsync(subscribedWebhooks.Callback, paymentResult);
    }
}