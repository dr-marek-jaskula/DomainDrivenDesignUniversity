namespace Shopway.Infrastructure.Payments;

public sealed class PaymentGatewayOptions
{
    public string? PublicKey { get; set; } = string.Empty;
    public string? SecretKey { get; set; } = string.Empty;
    public string? WebhookSecret { get; set; } = string.Empty;
}
