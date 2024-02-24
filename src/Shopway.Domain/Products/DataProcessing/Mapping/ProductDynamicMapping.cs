using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Domain.Common.Utilities;
using static Shopway.Domain.Constants.Constants.Mapping.Product;

namespace Shopway.Domain.Products.DataProcessing.Mapping;

public sealed record ProductDynamicMapping : IDynamicMapping<Product, DataTransferObject>
{
    public static IReadOnlyCollection<string> AllowedProperties { get; } = AllowedProductMappingProperties;

    public required IList<MappingEntry> MappingEntries { get; init; }

    public IQueryable<DataTransferObject> Apply(IQueryable<Product> queryable)
    {
        return queryable
            .Map(MappingEntries);
    }
}