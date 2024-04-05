using Shopway.Domain.Common.Errors;
using Shopway.Domain.Orders.Enumerations;

namespace Shopway.Domain.Orders.Errors;

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
            return Error.New($"{nameof(Status)}.{nameof(InvalidStatusChange)}", $"Cannot change status from {currentStatus} to {destinationStatus}.");
        }

        public static readonly Error PaymentNotReceived = Error.New($"{nameof(Status)}.{nameof(PaymentNotReceived)}", $"Payment was not received.");
    }
}