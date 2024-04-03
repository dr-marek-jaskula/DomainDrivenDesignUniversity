namespace PaymentGateway.Webhook;

public record Subscription(string Issuer, string Webhook, string WebhookSecret);
