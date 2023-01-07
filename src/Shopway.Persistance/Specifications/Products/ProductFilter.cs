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

    public bool ByProductName => ProductName.IsNullOrEmptyOrWhiteSpace();
    public bool ByRevision => Revision.IsNullOrEmptyOrWhiteSpace();
    public bool ByPrice => Price.HasValue;
    public bool ByUomCode => UomCode.IsNullOrEmptyOrWhiteSpace();

    public IQueryable<Product> Apply(IQueryable<Product> queryable)
    {
        return queryable
            .Filter(this);
    }
}