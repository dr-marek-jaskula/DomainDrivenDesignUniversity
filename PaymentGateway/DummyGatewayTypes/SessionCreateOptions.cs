namespace PaymentGateway.DummyGatewayTypes;

internal class SessionCreateOptions
{
    public List<SessionLineItemOptions> LineItems { get; set; } = [];
    public string Mode { get; set; } = string.Empty;
    public string SuccessUrl { get; set; } = string.Empty;
    public string CancelUrl { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
}