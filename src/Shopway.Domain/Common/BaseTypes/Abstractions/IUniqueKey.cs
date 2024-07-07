using System.Linq.Expressions;

namespace Shopway.Domain.Common.BaseTypes.Abstractions;

/// <summary>
/// Unique key that distinguishes the entity in the context of aggregate. Usually it is a composed key of entity's fields. Can contain an id
/// </summary>
/// 
public interface IUniqueKey
{
    const string Key = nameof(Key);
}

public interface IUniqueKey<TEntity, TUniqueKey> : IUniqueKey
    where TEntity : class, IEntity
    where TUniqueKey : IUniqueKey
{
    Expression<Func<TEntity, bool>> CreateQuerySpecification();

    abstract static TUniqueKey Create(Dictionary<string, string> key);
}
