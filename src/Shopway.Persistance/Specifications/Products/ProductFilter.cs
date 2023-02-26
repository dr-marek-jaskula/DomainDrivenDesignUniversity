using Shopway.Domain.Abstractions;
using Shopway.Domain.Entities;
using Shopway.Domain.Utilities;

namespace Shopway.Persistence.Specifications.Products;

public sealed record ProductFilter : IFilter<Product>
{
    public string? ProductName { get; init; }
    public string? Revision { get; init; }
    public decimal? Price { get; init; }
    public string? UomCode { get; init; }

    private bool ByProductName => ProductName.IsNotNullOrEmptyOrWhiteSpace();
    private bool ByRevision => Revision.IsNotNullOrEmptyOrWhiteSpace();
    private bool ByPrice => Price.HasValue;
    private bool ByUomCode => UomCode.IsNotNullOrEmptyOrWhiteSpace();

    public IQueryable<Product> Apply(IQueryable<Product> queryable)
    {
        return queryable
            .Filter(ByProductName, product => product.ProductName.Value.Contains(ProductName!))
            .Filter(ByRevision, product => product.Revision.Value.Contains(Revision!))
            .Filter(ByPrice, product => product.Price.Value == Price!)
            .Filter(ByUomCode, product => product.UomCode.Value.Contains(UomCode!));
    }
}