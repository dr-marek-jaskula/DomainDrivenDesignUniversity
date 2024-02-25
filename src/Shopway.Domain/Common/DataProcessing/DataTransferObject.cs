using Shopway.Domain.Common.Utilities;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace Shopway.Domain.Common.DataProcessing;

public class DataTransferObject : IDictionary<string, object>
{
    //Static Method infos
    private static readonly MethodInfo _appendMethodInfo = typeof(DataTransferObject)
        .GetMethod(nameof(Append))!;

    private static readonly MethodInfo _selectMethodInfo = typeof(Enumerable)
        .GetTypeInfo()
        .GetDeclaredMethods(nameof(Enumerable.Select))
        .First();

    private static readonly MethodInfo _createStringUsintInterpolationMethodInfo = typeof(DataTransferObject)
        .GetMethod(nameof(ToStringUsingInterpolation))!;

    //Static Expressions
    private static readonly NewExpression _newDtoExpression = Expression.New(typeof(DataTransferObject));

    protected readonly Dictionary<string, object> _dictionary = [];

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
            resultExpression = ChainAppendExpression(entity, resultExpression, mappingEntry);
        }

        return Expression.Lambda<Func<TEntity, DataTransferObject>>(resultExpression!, entity);
    }

    private static MethodCallExpression ChainAppendExpression(Expression parameter, Expression resultExpression, MappingEntry mappingEntry)
    {
        if (mappingEntry.PropertyName is not null)
        {
            return ChainProperty(parameter, resultExpression, mappingEntry);
        }

        if (mappingEntry.From is null || mappingEntry.Properties is null)
        {
            throw new ArgumentNullException("Missing From or Properties for mapping entry");
        }

        //For example: Product.Reviews or OrderHeader.Payment
        var nestedProperty = Expression.Property(parameter, mappingEntry.From);
        var nestedPropertyNameExpression = Expression.Constant(mappingEntry.From);
        var nestedPropertyType = ((PropertyInfo)nestedProperty.Member).PropertyType;

        if (nestedPropertyType.IsGeneric(out var notCollectionPropertyType))
        {
            return ChainAppendForCollectionProperty(resultExpression, mappingEntry, nestedProperty, nestedPropertyNameExpression, notCollectionPropertyType);
        }

        return ChainAppendForReferenceProperty(resultExpression, mappingEntry, nestedPropertyNameExpression, nestedProperty);
    }

    private static MethodCallExpression ChainAppendForReferenceProperty
    (
        Expression resultExpression,
        MappingEntry mappingEntry,
        ConstantExpression nestedPropertyNameExpression,
        Expression nestedProperty
    )
    {
        Expression resultOfNestedExpression = _newDtoExpression;

        foreach (var property in mappingEntry.Properties!)
        {
            resultOfNestedExpression = ChainAppendExpression(nestedProperty, resultOfNestedExpression, property);
        }

        return Expression.Call(resultExpression, _appendMethodInfo, nestedPropertyNameExpression, resultOfNestedExpression);
    }

    private static MethodCallExpression ChainAppendForCollectionProperty
    (
        Expression resultExpression,
        MappingEntry mappingEntry,
        MemberExpression nestedProperty,
        ConstantExpression nestedPropertyNameExpression,
        Type nestedPropertyType
    )
    {
        var nestedSelectorParameter = Expression.Parameter(nestedPropertyType, nestedPropertyType.Name);
        Expression? firstCallExpression = _newDtoExpression;

        foreach (var property in mappingEntry.Properties!)
        {
            firstCallExpression = ChainAppendExpression(nestedSelectorParameter, firstCallExpression, property);
        }

        LambdaExpression lambdaExpression = Expression.Lambda(firstCallExpression, false, nestedSelectorParameter);

        var genericSelectMethodInfo = _selectMethodInfo
            .MakeGenericMethod(nestedPropertyType, typeof(DataTransferObject));

        //Product.Reviews.Select(Review => new DataTransferObject().Append.Append...)
        var selectorCall = Expression.Call(null, genericSelectMethodInfo, nestedProperty, lambdaExpression);
        return Expression.Call(resultExpression, _appendMethodInfo, nestedPropertyNameExpression, selectorCall);
    }

    private static MethodCallExpression ChainProperty(Expression parameter, Expression resultExpression, MappingEntry mappingEntry)
    {
        var propertyNameExpression = Expression.Constant(mappingEntry.PropertyName);
        var propertyExpression = Expression.PropertyOrField(parameter, mappingEntry.PropertyName!);
        var type = ((PropertyInfo)propertyExpression.Member).PropertyType;

        //To avoid "Nullable object must have a value." we distinguish the string call
        MethodCallExpression propertyExpressionToString = type.IsValueType
            ? Expression.Call(propertyExpression, nameof(ToString), null)
            : Expression.Call(typeof(DataTransferObject), nameof(ToStringUsingInterpolation), [type], propertyExpression);

        return Expression.Call(resultExpression, _appendMethodInfo, propertyNameExpression, propertyExpressionToString);
    }

    public object this[string key] { get => _dictionary[key]; set => _dictionary[key] = value; }

    public ICollection<string> Keys => _dictionary.Keys;

    public ICollection<object> Values => _dictionary.Values;

    public int Count => _dictionary.Count;

    public bool IsReadOnly => false;

    public void Add(string key, object value)
    {
        _dictionary.Add(key, value);
    }

    public void Add(KeyValuePair<string, object> item)
    {
        _dictionary[item.Key] = item.Value;
    }

    public DataTransferObject AddIf(bool? condition, string key, object value)
    {
        if (condition is true && CanAdd(value))
        {
            Add(key, value);
        }

        return this;
    }

    public DataTransferObject Append(string key, object value)
    {
        if (CanAdd(value))
        {
            Add(key, value);
        }

        return this;
    }

    public void Clear()
    {
        _dictionary.Clear();
    }

    public bool Contains(KeyValuePair<string, object> item)
    {
        return _dictionary.Contains(item);
    }

    public bool ContainsKey(string key)
    {
        return _dictionary.ContainsKey(key);
    }

    public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    {
        return _dictionary.GetEnumerator();
    }

    public bool Remove(string key)
    {
        return _dictionary.Remove(key);
    }

    public bool Remove(KeyValuePair<string, object> item)
    {
        if (_dictionary.Contains(item))
        {
            return _dictionary.Remove(item.Key);
        }

        return false;
    }

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out object value)
    {
        return _dictionary.TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _dictionary.GetEnumerator();
    }

    private static bool CanAdd(object value)
    {
        if (value is null)
        {
            return false;
        }

        if (value is string @string && @string == string.Empty)
        {
            return false;
        }

        return true;
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