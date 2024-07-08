namespace Shopway.Domain.Common.BaseTypes.Abstractions;

public interface IEntityId<TEntityId> : IEntityId
{
    /// <summary>
    /// Create a new entity id using given ulid
    /// </summary>
    abstract static TEntityId Create(Ulid id);

    /// <summary>
    /// Create a new entity id using randomly generated ulid
    /// </summary>
    abstract static TEntityId New();

    static bool operator >(IEntityId<TEntityId> a, IEntityId<TEntityId> b) => a.CompareTo(b) is 1;
    static bool operator <(IEntityId<TEntityId> a, IEntityId<TEntityId> b) => a.CompareTo(b) is -1;
    static bool operator >=(IEntityId<TEntityId> a, IEntityId<TEntityId> b) => a.CompareTo(b) >= 0;
    static bool operator <=(IEntityId<TEntityId> a, IEntityId<TEntityId> b) => a.CompareTo(b) <= 0;
}

public interface IEntityId : IComparable<IEntityId>
{
    const string Id = nameof(Id);
    Ulid Value { get; }

    static bool operator >(IEntityId a, IEntityId b) => a.CompareTo(b) is 1;
    static bool operator <(IEntityId a, IEntityId b) => a.CompareTo(b) is -1;
    static bool operator >=(IEntityId a, IEntityId b) => a.CompareTo(b) >= 0;
    static bool operator <=(IEntityId a, IEntityId b) => a.CompareTo(b) <= 0;
}
