using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
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