using Shopway.Domain.Common.Utilities;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace Shopway.Domain.Common.DataProcessing;

public class DataTransferObject : IDictionary<string, object?>
{
    //Static method infos
    private static readonly MethodInfo _appendMethodInfo = typeof(DataTransferObject)
        .GetMethod(nameof(Append))!;

    private static readonly MethodInfo _selectMethodInfo = typeof(Enumerable)
        .GetTypeInfo()
        .GetDeclaredMethods(nameof(Enumerable.Select))
        .First();

    //Static expressions
    private static readonly NewExpression _newDtoExpression = Expression.New(typeof(DataTransferObject));

    protected readonly Dictionary<string, object?> _dictionary = [];

    public DataTransferObject()
    {
    }

    protected DataTransferObject(DataTransferObject dataTransferObject)
    {
        _dictionary = dataTransferObject._dictionary;
    }

    public static Expression<Func<TEntity, DataTransferObject>> CreateExpression<TEntity>(IList<MappingEntry> mappingEntries)
    {
        var entity = Expression.Parameter(typeof(TEntity), typeof(TEntity).Name);
        Expression resultExpression = _newDtoExpression;

        foreach (var mappingEntry in mappingEntries)
        {
            resultExpression = ChainAppendExpression(resultExpression, mappingEntry, entity);
        }

        return Expression.Lambda<Func<TEntity, DataTransferObject>>(resultExpression!, entity);
    }

    private static MethodCallExpression ChainAppendExpression
    (
        Expression resultExpression, 
        MappingEntry mappingEntry, 
        Expression parameter
    )
    {
        if (mappingEntry.PropertyName is not null)
        {
            return ChainAppendForProperty(resultExpression, mappingEntry, parameter);
        }

        if (mappingEntry.From is null || mappingEntry.Properties is null)
        {
            throw new ArgumentNullException("Missing From or Properties for mapping entry");
        }

        //For example: Product.Reviews or OrderHeader.Payment
        var nestedProperty = Expression.Property(parameter, mappingEntry.From);

        if (nestedProperty.IsGeneric(out var genericPropertyType))
        {
            return ChainAppendForCollectionProperty(resultExpression, mappingEntry, nestedProperty, genericPropertyType);
        }

        return ChainAppendForReferenceProperty(resultExpression, mappingEntry, nestedProperty);
    }

    private static MethodCallExpression ChainAppendForProperty
    (
        Expression resultExpression,
        MappingEntry mappingEntry,
        Expression parameter
    )
    {
        var propertyNameExpression = Expression.Constant(mappingEntry.PropertyName);
        var propertyExpression = Expression.PropertyOrField(parameter, mappingEntry.PropertyName!);
        var propertyType = ((PropertyInfo)propertyExpression.Member).PropertyType;

        //To avoid "Nullable object must have a value." we distinguish the string call
        MethodCallExpression propertyExpressionToString = propertyType.IsValueType
            ? Expression.Call(propertyExpression, nameof(ToString), null)
            : Expression.Call(typeof(DataTransferObject), nameof(ToStringUsingInterpolation), [propertyType], propertyExpression);

        return Expression.Call(resultExpression, _appendMethodInfo, propertyNameExpression, propertyExpressionToString);
    }

    private static MethodCallExpression ChainAppendForReferenceProperty
    (
        Expression resultExpression,
        MappingEntry mappingEntry,
        Expression nestedProperty
    )
    {
        Expression resultOfNestedExpression = _newDtoExpression;

        foreach (var nestedMappingEntry in mappingEntry.Properties!)
        {
            resultOfNestedExpression = ChainAppendExpression(resultOfNestedExpression, nestedMappingEntry, nestedProperty);
        }

        return Expression.Call(resultExpression, _appendMethodInfo, Expression.Constant(mappingEntry.From), resultOfNestedExpression);
    }

    private static MethodCallExpression ChainAppendForCollectionProperty
    (
        Expression resultExpression,
        MappingEntry mappingEntry,
        MemberExpression nestedProperty,
        Type nestedPropertyType
    )
    {
        var nestedSelectParameter = Expression.Parameter(nestedPropertyType, nestedPropertyType.Name);
        Expression? collectionResultExpression = _newDtoExpression;

        foreach (var nestedMappingEntry in mappingEntry.Properties!)
        {
            collectionResultExpression = ChainAppendExpression(collectionResultExpression, nestedMappingEntry, nestedSelectParameter);
        }

        LambdaExpression lambdaExpression = Expression.Lambda(collectionResultExpression, false, nestedSelectParameter);

        var genericSelectMethodInfo = _selectMethodInfo
            .MakeGenericMethod(nestedPropertyType, typeof(DataTransferObject));

        //Product.Reviews.Select(Review => new DataTransferObject().Append.Append...)
        var selectCallExpression = Expression.Call(null, genericSelectMethodInfo, nestedProperty, lambdaExpression);
        return Expression.Call(resultExpression, _appendMethodInfo, Expression.Constant(mappingEntry.From), selectCallExpression);
    }

    public object? this[string key] { get => _dictionary[key]; set => _dictionary[key] = value; }

    public ICollection<string> Keys => _dictionary.Keys;

    public ICollection<object?> Values => _dictionary.Values;

    public int Count => _dictionary.Count;

    public bool IsReadOnly => false;

    public void Add(string key, object? value)
    {
        _dictionary.Add(key, value);
    }

    public void Add(KeyValuePair<string, object?> item)
    {
        _dictionary[item.Key] = item.Value;
    }

    public DataTransferObject AddIf(bool? condition, string key, object? value)
    {
        if (condition is true)
        {
            Append(key, value);
        }

        return this;
    }

    public DataTransferObject Append(string key, object? value)
    {
        if (value.IsNullOrEmptyString())
        {
            return this;
        }

        if (value is DataTransferObject dto && dto.ShouldBeSetToNull())
        {
            Add(key, null);
            return this;
        }

        Add(key, value);
        return this;
    }

    private bool ShouldBeSetToNull()
    {
        return _dictionary.Count is 0
            || _dictionary.All(x => x.Value is null);
    }

    public void Clear()
    {
        _dictionary.Clear();
    }

    public bool Contains(KeyValuePair<string, object?> item)
    {
        return _dictionary.Contains(item);
    }

    public bool ContainsKey(string key)
    {
        return _dictionary.ContainsKey(key);
    }

    public void CopyTo(KeyValuePair<string, object?>[] array, int arrayIndex)
    {
        ((IDictionary<string, object?>)_dictionary).CopyTo(array, arrayIndex);
    }

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        return _dictionary.GetEnumerator();
    }

    public bool Remove(string key)
    {
        return _dictionary.Remove(key);
    }

    public bool Remove(KeyValuePair<string, object?> item)
    {
        if (_dictionary.Contains(item))
        {
            return _dictionary.Remove(item.Key);
        }

        return false;
    }

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out object? value)
    {
        return _dictionary.TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _dictionary.GetEnumerator();
    }

    /// <summary>
    /// This approach is equivalent to:
    /// var handler = new DefaultInterpolatedStringHandler();
    /// handler.AppendFormatted(value);
    /// return handler.ToStringAndClear();
    /// </summary>
    public static string ToStringUsingInterpolation<T>(T value)
    {
        return $"{value}";
    }
}