using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Orders;
using Shopway.Domain.Users;
using System.Collections.Frozen;

namespace Shopway.Persistence.Specifications.OrderHeaders;

internal static partial class OrderHeaderSpecification
{
    internal static partial class ById
    {
        public static partial class WithOrderLines
        {
            public static partial class AndProducts
            {
                //Static cache, use in case when performance improvements are required. Used for demonstration purposes. Can be use as simple array or FrozenSet
                private static readonly FrozenSet<IncludeEntry<OrderHeader>> _buildIncludesFrozen = IncludeBuilderOrchestrator<OrderHeader>
                    .GetIncludeEntries(builder => builder.Include(orderHeader => orderHeader.OrderLines))
                    .ToFrozenSet();

                internal static Specification<OrderHeader, OrderHeaderId> Create(OrderHeaderId orderHeaderId, bool includePayment = true)
                {
                    var specification = Specification<OrderHeader, OrderHeaderId>.New()
                        .AddIncludes(_buildIncludesFrozen)
                        //Use the commented code as baseline. Use cached IncludeEntries when performance improvements are required
                        //.AddIncludes(builder => builder
                        //    .Include(orderHeader => orderHeader.OrderLines))
                        .AddFilters(orderHeader => orderHeader.Id == orderHeaderId);

                    if (includePayment)
                    {
                        specification.AddIncludes(orderHeader => orderHeader.Payment);
                    }

                    return specification;
                }
            }
        }

        public static partial class AndUserId
        {
            internal static Specification<OrderHeader, OrderHeaderId> Create(OrderHeaderId orderHeaderId, UserId userId)
            {
                var specification = Specification<OrderHeader, OrderHeaderId>.New()
                    .AddFilters(orderHeader => orderHeader.Id == orderHeaderId, orderHeader => orderHeader.UserId == userId);

                return specification;
            }
        }
    }
}