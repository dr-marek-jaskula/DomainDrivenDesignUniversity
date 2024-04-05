namespace Shopway.Infrastructure.Payments.DummyGatewayTypes;

public class Session
{
    public string Id { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string PaymentIntentId { get; set; } = string.Empty;
}