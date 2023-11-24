using Shopway.Persistence.Abstractions;
using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;

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
    internal static SpecificationWithMappingBase<TEntity, TEntityId, TResponse> AsMappingSpecification<TEntity, TEntityId, TResponse>(this SpecificationBase<TEntity, TEntityId> specification)
        where TEntityId : struct, IEntityId<TEntityId>
        where TEntity : Entity<TEntityId>
    {
        return (SpecificationWithMappingBase<TEntity, TEntityId, TResponse>)specification;
    }
}