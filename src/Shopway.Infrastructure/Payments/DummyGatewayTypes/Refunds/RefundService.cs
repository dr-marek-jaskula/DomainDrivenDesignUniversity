namespace Shopway.Infrastructure.Payments.DummyGatewayTypes.Refunds;

public class RefundService
{
    public Task CreateAsync(RefundCreateOptions refundCreateOptions)
    {
        return Task.CompletedTask;
    }
}