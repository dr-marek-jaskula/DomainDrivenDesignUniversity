using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Persistence.Abstractions;
namespace Shopway.Persistence.Utilities;

public static class SpecificationUtilities
{
    public static SpecificationWithMappingBase<Product, ProductId, TResponse> AsMappingSpecification<TResponse>(this SpecificationBase<Product, ProductId> specification)
    {
        return (SpecificationWithMappingBase<Product, ProductId, TResponse>)specification;
    }
}