namespace Shopway.Domain.Orders.Enumerations;

public static class PaymentStatusUtilities
{
    public static bool IsReceivedOrConfirmed(this PaymentStatus paymentStatus)
    {
        return paymentStatus is PaymentStatus.Received or PaymentStatus.Confirmed;
    }
}
