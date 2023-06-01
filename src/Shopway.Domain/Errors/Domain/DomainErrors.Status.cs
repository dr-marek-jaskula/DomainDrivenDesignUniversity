using Shopway.Domain.Enums;

namespace Shopway.Domain.Errors.Domain;

public static partial class DomainErrors
{
    public static class Status
    {
        /// <summary>
        /// Create an Error based on the order status change
        /// </summary>
        /// <returns>InvalidStatusChange error</returns>
        public static Error InvalidStatusChange(OrderStatus currentStatus, OrderStatus destinationStatus)
        {
            return new($"{nameof(Status)}.{nameof(InvalidStatusChange)}", $"Cannot change status from {currentStatus} to {destinationStatus}.");
        }

        public static readonly Error PaymentNotReceived = new($"{nameof(Status)}.{nameof(PaymentNotReceived)}", $"Payment was not received.");
    }
}