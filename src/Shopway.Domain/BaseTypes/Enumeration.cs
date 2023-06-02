using Shopway.Domain.Exceptions;
using System.Reflection;

namespace Shopway.Domain.BaseTypes;

/// <summary>
/// Represents an enumeration of objects with a unique numeric identifier and a name
/// </summary>
/// <typeparam name="TEnum">The type of the enumeration</typeparam>
public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>>, IComparable<Enumeration<TEnum>>
    where TEnum : Enumeration<TEnum>
{
    private static readonly string EnumerationName = typeof(TEnum).Name;
    private static readonly Lazy<Dictionary<int, TEnum>> EnumerationsDictionary =
        new(() => CreateEnumerationDictionary(typeof(TEnum)));

    /// <summary>
    /// Initializes a new instance of the <see cref="Enumeration{TEnum}"/> class
    /// </summary>
    /// <param name="id">The enumeration identifier</param>
    /// <param name="name">The enumeration name</param>
    protected Enumeration(int id, string name)
        : this()
    {
        Id = id;
        Name = name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Enumeration{TEnum}"/> class
    /// </summary>
    /// <remarks>
    /// Required for deserialization
    /// </remarks>
    protected Enumeration()
    {
        Id = default;
        Name = string.Empty;
    }

    public static IReadOnlyCollection<TEnum> List => EnumerationsDictionary.Value.Values.ToList().AsReadOnly();
    public static IReadOnlyCollection<int> Ids => EnumerationsDictionary.Value.Keys.ToList().AsReadOnly();

    /// <summary>
    /// Gets the identifier
    /// </summary>
    public int Id { get; protected init; }

    /// <summary>
    /// Gets the name
    /// </summary>
    public string Name { get; protected init; }

    public static bool operator ==(Enumeration<TEnum>? a, Enumeration<TEnum>? b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(Enumeration<TEnum> a, Enumeration<TEnum> b)
    {
        return !(a == b);
    }

    /// <summary>
    /// Creates an enumeration of the specified type based on the specified identifier
    /// </summary>
    /// <param name="id">The enumeration identifier</param>
    /// <returns>The enumeration instance that matches the specified identifier, if it exists</returns>
    public static TEnum? FromId(int id)
    {
        var isValueInDictionary = EnumerationsDictionary
            .Value
            .TryGetValue(id, out TEnum? enumeration);

        return isValueInDictionary
            ? enumeration
            : throw new EnumerationNotFoundException(EnumerationName, id);
    }

    /// <summary>
    /// Creates an enumeration of the specified type based on the specified name
    /// </summary>
    /// <param name="name">The enumeration name</param>
    /// <returns>The enumeration instance that matches the specified name, if it exists</returns>
    public static TEnum? FromName(string name)
    {
        return EnumerationsDictionary
            .Value
            .Values
            .SingleOrDefault(x => x.Name == name);
    }

    /// <summary>
    /// Checks if the enumeration with the specified identifier exists.
    /// </summary>
    /// <param name="id">The enumeration identifier.</param>
    /// <returns>True if an enumeration with the specified identifier exists, otherwise false.</returns>
    public static bool Contains(int id)
    {
        return EnumerationsDictionary
            .Value
            .ContainsKey(id);
    }

    /// <inheritdoc />
    public virtual bool Equals(Enumeration<TEnum>? other)
    {
        if (other is null)
        {
            return false;
        }

        return GetType() == other.GetType() && other.Id.Equals(Id);
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

        return obj is Enumeration<TEnum> otherValue && otherValue.Id.Equals(Id);
    }

    /// <inheritdoc />
    public int CompareTo(Enumeration<TEnum>? other)
    {
        return other is null
            ? 1
            : Id.CompareTo(other.Id);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Id.GetHashCode() * 37;
    }

    /// <inheritdoc />
    public static HashSet<string> GetNames()
    {
        return List
            .Select(p => p.Name)
            .ToHashSet();
    }

    private static Dictionary<int, TEnum> CreateEnumerationDictionary(Type enumType)
    {
        return GetFieldsForType(enumType)
            .ToDictionary(t => t.Id);
    }

    private static IEnumerable<TEnum> GetFieldsForType(Type enumType)
    {
        return enumType
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fieldInfo => enumType.IsAssignableFrom(fieldInfo.FieldType))
            .Select(fieldInfo => (TEnum)fieldInfo.GetValue(default)!);
    }
}
