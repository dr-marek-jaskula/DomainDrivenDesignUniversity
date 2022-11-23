namespace Shopway.Domain.Primitives;

public interface IStronglyTypedId
{
    public Guid Value { get; init; }
}

internal readonly record struct StronglyTypedId2
{
    public static readonly StronglyTypedId Empty = new(System.Guid.NewGuid());

	public StronglyTypedId2(Guid id)
	{
        Value = id;
    }

    public Guid Value { get; init; }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}

public class StronglyTypedId : IEquatable<StronglyTypedId>
{
    public Guid Value { get; }

    public StronglyTypedId(Guid value)
    {
        Value = value;
    }

    public bool Equals(StronglyTypedId? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return EqualityComparer<Guid>.Default.Equals(Value, other.Value);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((StronglyTypedId)obj);
    }

    public override int GetHashCode()
    {
        return EqualityComparer<Guid>.Default.GetHashCode(Value);
    }

    public static bool operator ==(StronglyTypedId? left, StronglyTypedId? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(StronglyTypedId? left, StronglyTypedId? right)
    {
        return !Equals(left, right);
    }
}

/*
It's easy if you create Entity<TId> where TId : IEntityId

And then create a strongly typed Id that inherits IEntityId

And this is just marker interface:

public interface IEntityId { }

I also suggest using records for strongly typed Ids, for value comparison

But... You'll have trouble with EF mapping. So be aware of that.
 */