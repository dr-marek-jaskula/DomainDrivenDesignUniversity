using Shopway.Domain.Common.Utilities;

namespace Shopway.Domain.Common.BaseTypes;

/// <summary>
/// Each ValueObject should contain at least two public, static methods: "Create" and "Validate".
/// "Create" method should return the ValidationResult of same ValueObjectType and should use the "Validate" method.
/// "Validate" method should return List<Error> and contain all value object validation
/// </summary>
[Serializable]
public abstract class ValueObject : IEquatable<ValueObject>
{
    public const string Value = nameof(Value);

    //For instance the atomic value of a Email is a single character
    /// <summary>
    /// Gets the atomic values of the value object.
    /// </summary>
    /// <returns>The collection of objects representing the value object values.</returns>
    public abstract IEnumerable<object> GetAtomicValues();

    //ValueObjects are equal when their values are equal
    /// <inheritdoc />
    public bool Equals(ValueObject? other)
    {
        return other is not null && ValuesAreEqual(other);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (GetType() != obj.GetType())
        {
            return false;
        }

        if (obj is not ValueObject otherValueObject)
        {
            return false;
        }

        return ValuesAreEqual(otherValueObject);
    }

    public static bool operator ==(ValueObject? first, ValueObject? second)
    {
        if (first is null && second is null)
        {
            return true;
        }

        if (first is null || second is null)
        {
            return false;
        }

        return first.Equals(second);
    }

    public static bool operator !=(ValueObject? first, ValueObject? second)
    {
        return !(first == second);
    }

    //Make a hash using the whole AtomicValues collection (and the initial value of default(int) which is zero)
    public override int GetHashCode()
    {
        return GetAtomicValues()
            .Aggregate(default(int), (hashcode, value) => HashCode.Combine(hashcode, value.GetHashCode()));
    }

    /// <summary>
    /// Checks if the values of the specified value object are equal to the values of the current instance.
    /// </summary>
    /// <param name="other">The other value object.</param>
    /// <returns>True if the values of the specified value object are equal to the values of the current instance, otherwise false.</returns>
    private bool ValuesAreEqual(ValueObject other)
    {
        return GetAtomicValues()
            .SequenceEqual(other.GetAtomicValues());
    }

    public override string ToString()
    {
        return GetAtomicValues()
            .Join(", ");
    }
}
