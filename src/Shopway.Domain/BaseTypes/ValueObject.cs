namespace Shopway.Domain.BaseTypes;

[Serializable]
public abstract class ValueObject : IEquatable<ValueObject>
{
    //For instance the atomic value of a Email is a single character
    public abstract IEnumerable<object> GetAtomicValues();

    //ValueObjects are equal when their values are equal
    public bool Equals(ValueObject? other)
    {
        return other is not null && ValuesAreEqual(other);
    }

    public override bool Equals(object? obj)
    {
        return obj is ValueObject other && ValuesAreEqual(other);
    }

    //Make a hash using the whole AtomicValues collection (and the initial value of default(int) which is zero)
    public override int GetHashCode()
    {
        return GetAtomicValues()
            .Aggregate(default(int), HashCode.Combine);
    }

    private bool ValuesAreEqual(ValueObject other)
    {
        return GetAtomicValues()
            .SequenceEqual(other.GetAtomicValues());
    }
}