namespace PaymentGateway.DummyGatewayTypes;

public class SessionLineItemOptions
{
    public int Quantity { get; internal set; }
    public SessionLineItemPriceDataOptions? PriceData { get; internal set; }
}