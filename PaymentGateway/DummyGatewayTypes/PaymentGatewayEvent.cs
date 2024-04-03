namespace PaymentGateway.DummyGatewayTypes;

public class PaymentGatewayEvent
{
    public Data Data { get; internal set; } = null!;
    public string Type { get; internal set; } = string.Empty;
}

public class Data
{
    public Session Object { get; set; } = null!;
}
