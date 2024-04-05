namespace Shopway.Infrastructure.Payments.DummyGatewayTypes.Sessions;

public class SessionLineItemOptions
{
    public int Quantity { get; internal set; }
    public SessionLineItemPriceDataOptions? PriceData { get; internal set; }
}