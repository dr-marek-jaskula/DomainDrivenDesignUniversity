using Shopway.Domain.Common.DataProcessing.Abstractions;
using System.Linq.Expressions;

namespace Shopway.Domain.Products.DataProcessing.Filtering;

public sealed record ProductFuzzyFilter : IFilter<Product>
{
    public required Expression<Func<Product, bool>> FuzzyFilter { get; set; }

    public IQueryable<Product> Apply(IQueryable<Product> queryable, ILikeProvider<Product>? createLikeProvider = null)
    {
        return queryable
            .Where(FuzzyFilter);
    }
}
