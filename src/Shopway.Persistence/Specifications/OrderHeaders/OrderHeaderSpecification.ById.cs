using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Microsoft.EntityFrameworkCore;
using Shopway.Persistence.Abstractions;

namespace Shopway.Persistence.Specifications.OrderHeaders;

internal abstract partial class OrderHeaderSpecification 
{
    internal sealed partial class ById : SpecificationBase<OrderHeader, OrderHeaderId>
    {
        public sealed class WithOrderLines: SpecificationBase<OrderHeader, OrderHeaderId>
        {
            public sealed class AndProducts : SpecificationBase<OrderHeader, OrderHeaderId>
            {
                private AndProducts() { }

                internal static SpecificationBase<OrderHeader, OrderHeaderId> Create(OrderHeaderId orderHeaderId, bool includePayment = true)
                {
                    var specification = new AndProducts()
                        .AddIncludesWithThenIncludesAction(orderHeader => orderHeader.Include(o => o.OrderLines).ThenInclude(od => od.Product))
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