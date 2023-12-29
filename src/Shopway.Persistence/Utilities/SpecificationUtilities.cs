using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Persistence.Specifications;

namespace Shopway.Persistence.Utilities;

internal static class SpecificationUtilities
{
    /// <summary>
    /// Cast specification to mapping specification
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TEntityId">EntityId type</typeparam>
    /// <typeparam name="TResponse">Response type</typeparam>
    /// <param name="specification">Input specification</param>
    /// <returns>SpecificationWithMappingBase</returns>
    internal static SpecificationWithMapping<TEntity, TEntityId, TResponse> AsMappingSpecification<TEntity, TEntityId, TResponse>(this Specification<TEntity, TEntityId> specification)
        where TEntityId : struct, IEntityId<TEntityId>
        where TEntity : Entity<TEntityId>
    {
        return (SpecificationWithMapping<TEntity, TEntityId, TResponse>)specification;
    }
}