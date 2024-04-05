namespace Shopway.Infrastructure.Payments.DummyGatewayTypes.Events;

public static class Events
{
    internal const string CheckoutSessionCompleted = nameof(CheckoutSessionCompleted);
    internal const string CheckoutSessionAsyncPaymentSucceeded = nameof(CheckoutSessionAsyncPaymentSucceeded);
    internal const string CheckoutSessionAsyncPaymentFailed = nameof(CheckoutSessionAsyncPaymentFailed);
}
