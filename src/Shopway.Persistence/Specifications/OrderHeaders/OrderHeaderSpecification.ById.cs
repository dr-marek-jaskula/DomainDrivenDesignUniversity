using Shopway.Domain.Orders;
using Shopway.Domain.Entities;

namespace Shopway.Persistence.Specifications.OrderHeaders;

internal static partial class OrderHeaderSpecification 
{
    internal static partial class ById
    {
        public static partial class WithOrderLines
        {
            public static partial class AndProducts
            {
                internal static Specification<OrderHeader, OrderHeaderId> Create(OrderHeaderId orderHeaderId, bool includePayment = true)
                {
                    var specification = Specification<OrderHeader, OrderHeaderId>.New()
                        .AddIncludes(orderHeader => orderHeader.OrderLines)
                        .AddFilters(product => product.Id == orderHeaderId);

                    if (includePayment)
                    {
                        specification.AddIncludes(orderHeader => orderHeader.Payment);
                    }

                    return specification;
                }
            }
        }
    }
}