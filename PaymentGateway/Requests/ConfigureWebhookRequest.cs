using PaymentGateway.Webhook;

namespace PaymentGateway.Requests;

public sealed record ConfigureWebhookRequest(string SecretHash, Subscription Subscription);
