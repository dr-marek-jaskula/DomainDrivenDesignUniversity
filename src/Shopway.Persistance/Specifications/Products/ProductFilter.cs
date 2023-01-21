using Shopway.Domain.Abstractions;
using Shopway.Domain.Entities;
using Shopway.Domain.Utilities;

namespace Shopway.Persistence.Specifications.Products;

public sealed record ProductFilter : IFilter<Product>
{
    public string? ProductName { get; init; }
    public string? Revision { get; init; }
    public int? Price { get; init; }
    public string? UomCode { get; init; }

    internal bool ByProductName => ProductName.IsNotNullOrEmptyOrWhiteSpace();
    internal bool ByRevision => Revision.IsNotNullOrEmptyOrWhiteSpace();
    internal bool ByPrice => Price.HasValue;
    internal bool ByUomCode => UomCode.IsNotNullOrEmptyOrWhiteSpace();

    public IQueryable<Product> Apply(IQueryable<Product> queryable)
    {
        return queryable
            .Filter(this);
    }
}