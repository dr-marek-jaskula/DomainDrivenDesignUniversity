namespace PaymentGateway.DummyGatewayTypes;

public class Session
{
    public string Id { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string Url { get; internal set; } = string.Empty;
}