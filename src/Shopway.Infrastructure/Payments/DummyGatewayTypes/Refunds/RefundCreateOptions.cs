namespace Shopway.Infrastructure.Payments.DummyGatewayTypes.Refunds;

public class RefundCreateOptions
{
    public string PaymentIntent { get; set; } = string.Empty;
}