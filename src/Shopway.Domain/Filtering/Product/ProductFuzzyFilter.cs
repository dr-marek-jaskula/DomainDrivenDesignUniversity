using System.Linq.Expressions;
using Shopway.Domain.Entities;
using Shopway.Domain.Utilities;
using Shopway.Domain.Abstractions.Common;

namespace Shopway.Domain.Filering.Products;

public sealed record ProductFuzzyFilter : IFilter<Product>
{
    public required Expression<Func<Product, bool>> FuzzyFilter { get; set; }

    public IQueryable<Product> Apply(IQueryable<Product> queryable)
    {
        return queryable
            .Where(FuzzyFilter);
    }
}