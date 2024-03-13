using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Domain.Common.Utilities;
using static Shopway.Domain.Constants.Constants.Mapping.OrderHeader;

namespace Shopway.Domain.Orders.DataProcessing.Mapping;

public sealed record OrderHeaderDynamicMapping : IDynamicMapping<OrderHeader>
{
    public static IReadOnlyCollection<string> AllowedProperties { get; } = AllowedOrderHeaderMappingProperties;

    public IList<MappingEntry> MappingEntries { get; init; } = [];

    public IQueryable<DataTransferObject> Apply(IQueryable<OrderHeader> queryable)
    {
        return queryable
            .Map(MappingEntries);
    }
}