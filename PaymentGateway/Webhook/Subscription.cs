namespace PaymentGateway.Webhook;

public record Subscription(string Issuer, string SessionId, string Callback);
