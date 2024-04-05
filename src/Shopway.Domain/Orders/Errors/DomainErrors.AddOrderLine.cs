using Shopway.Domain.Common.Errors;
using Shopway.Domain.Orders.Enumerations;

namespace Shopway.Domain.Orders.Errors;

public static partial class DomainErrors
{
    public static class AddOrderLineError
    {
        public static readonly Error InvalidOrderHeaderStatus = Error.InvalidOperation($"Adding lines to OrderHeader is allowed only for OrderHeader in {OrderStatus.New} status");
    }
}