namespace PaymentGateway.DummyGatewayTypes;

public static class PaymentGatewayConfiguration
{
    //Set it once at startup
    public static string ApiKey { get; set; } = string.Empty;
}