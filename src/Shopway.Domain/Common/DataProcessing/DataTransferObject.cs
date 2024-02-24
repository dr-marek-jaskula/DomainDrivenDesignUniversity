using Shopway.Domain.Common.Utilities;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace Shopway.Domain.Common.DataProcessing;

public class DataTransferObject : IDictionary<string, object>
{
    private static readonly MethodInfo _appendMethodInfo = typeof(DataTransferObject)
        .GetMethod(nameof(Append))!;

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
        NewExpression newDtoExpression = Expression.New(typeof(DataTransferObject));
        Expression? firstCallExpression = newDtoExpression;

        foreach (var mappingEntry in mappingEntries)
        {
            firstCallExpression = ChainAppendExpression(entity, firstCallExpression, mappingEntry);
        }

        var lambdaExpression = Expression.Lambda<Func<TEntity, DataTransferObject>>(firstCallExpression!, entity);

        return lambdaExpression;
    }

    private static MethodCallExpression ChainAppendExpression(ParameterExpression parameter, Expression expression, MappingEntry mappingEntry)
    {
        if (mappingEntry.PropertyName is not null)
        {
            var propertyNameExpression = Expression.Constant(mappingEntry.PropertyName);
            var propertyExpression = Expression.PropertyOrField(parameter, mappingEntry.PropertyName!);
            var propertyExpressionToString = Expression.Call(propertyExpression, nameof(ToString), null);
            return Expression.Call(expression, _appendMethodInfo, propertyNameExpression, propertyExpressionToString);
        }

        if (mappingEntry.From is null)
        {
            throw new ArgumentNullException(nameof(mappingEntry.From));
        }

        //For example: Product.Reviews
        var entityNestedMember = parameter.ToMemberExpression(mappingEntry.From);
        var entityNestedPropertyNameExpression = Expression.Constant(mappingEntry.From);
        var memberAsPropertyInfo = ((PropertyInfo)entityNestedMember.Member).PropertyType;

        if (memberAsPropertyInfo.IsGeneric(out var nestedEntityType))
        {
            if (mappingEntry.Properties is null)
            {
                throw new ArgumentNullException(nameof(mappingEntry.Properties));
            }

            var nestedSelectorParameter = Expression.Parameter(nestedEntityType, nestedEntityType.Name);
            NewExpression newDtoExpression = Expression.New(typeof(DataTransferObject));
            Expression? firstCallExpression = newDtoExpression;

            foreach (var property in mappingEntry.Properties)
            {
                firstCallExpression = ChainAppendExpression(nestedSelectorParameter, firstCallExpression, property);
            }

            //For example: Review => new DataTransferObject()
            LambdaExpression lambdaExpression = Expression.Lambda(firstCallExpression, false, nestedSelectorParameter);

            //Get select method
            var selectMethodInfo = typeof(Enumerable)
                .GetTypeInfo().GetDeclaredMethods(nameof(Enumerable.Select))
                .First()
                .MakeGenericMethod(nestedEntityType, typeof(DataTransferObject));

            //Product.Reviews.Select(Review => new DataTransferObject().Append.Append...)
            var selectorCall = Expression.Call(null, selectMethodInfo, entityNestedMember, lambdaExpression);

            return Expression.Call(expression, _appendMethodInfo, entityNestedPropertyNameExpression, selectorCall);
        }

        throw new NotImplementedException();
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
        if (condition is true)
        {
            Add(key, value);
        }

        return this;
    }

    public DataTransferObject Append(string key, object value)
    {
        Add(key, value);
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
}