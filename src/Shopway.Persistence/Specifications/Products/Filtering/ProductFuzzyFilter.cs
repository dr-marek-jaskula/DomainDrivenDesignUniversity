using System.Linq.Expressions;
using Shopway.Domain.Entities;
using Shopway.Domain.Utilities;
using Shopway.Domain.Abstractions.Common;

namespace Shopway.Persistence.Specifications.Products.Filtering;

public sealed record ProductFuzzyFilter : IStaticFilter<Product>
{
    public required Expression<Func<Product, bool>> FuzzyFilter { get; set; }

    public IQueryable<Product> Apply(IQueryable<Product> queryable)
    {
        return queryable
            .Where(FuzzyFilter);
    }
}