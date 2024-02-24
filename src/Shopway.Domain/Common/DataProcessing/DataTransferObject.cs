using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Products;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace Shopway.Domain.Common.DataProcessing;

public class DataTransferObject : IDictionary<string, object>
{
    //private static readonly MethodInfo _selectMethodInfo = typeof(Enumerable)
    //    .GetTypeInfo().GetDeclaredMethods(nameof(Enumerable.Select))
    //    .Single(mi => mi.GetGenericArguments().Length is 2
    //        && mi.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(IEnumerable<>)
    //        && mi.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>));

    private static readonly MethodInfo _createMethod = typeof(DataTransferObject)
        .GetSingleGenericMethod
        (
            nameof(Create),
            typeof(Product)
        );

    protected readonly Dictionary<string, object> _dictionary = [];

    public DataTransferObject()
    {
    }

    protected DataTransferObject(DataTransferObject dataTransferObject)
    {
        _dictionary = dataTransferObject._dictionary;
    }

    public static DataTransferObject Create<TEntity>(TEntity entity, IList<MappingEntry> mappingEntries)
        where TEntity : class, IEntity
    {
        var dto = new DataTransferObject();

        foreach (var mappingEntry in mappingEntries)
        {
            if (mappingEntry.PropertyName is not null)
            {
                var property = entity.GetProperty(mappingEntry.PropertyName);
                dto.Add(mappingEntry.PropertyName, $"{property}");
                continue;
            }

            if (mappingEntry.From is null)
            {
                throw new ArgumentNullException(nameof(mappingEntry.From));
            }

            var nestedEntityOrCollectionOfEntities = entity.GetProperty(mappingEntry.From);

            if (nestedEntityOrCollectionOfEntities.GetType().IsGeneric(out var nestedEntityType))
            {
                object nestedDto = CreateNestedDataTransferObject<TEntity>(mappingEntry, nestedEntityOrCollectionOfEntities, nestedEntityType);
                dto.Add(mappingEntry.From!, nestedDto);
                continue;
            }

            dto.Add(mappingEntry.From!, nestedEntityOrCollectionOfEntities);
        }

        return dto;
    }

    private static object CreateNestedDataTransferObject<TEntity>(MappingEntry mappingEntry, object nestedEntityOrCollectionOfEntities, Type nestedEntityType) 
        where TEntity : class, IEntity
    {
        var constantMappingProperties = Expression.Constant(mappingEntry.Properties);

        var nestedSelectorParameter = Expression.Parameter(nestedEntityType, nestedEntityType.Name);
        //use constant instead of : var mappingEntriesForNestedSelectorParameter = Expression.Parameter(typeof(IList<MappingEntry>), "MappingEntries");

        var createMethodInfo = typeof(DataTransferObject)
            .GetSingleGenericMethod(nameof(Create), nestedEntityType);

        //For example: Create<Review>(Review Review, IList<MappingEntry> MappingEntries)
        var createDataTransferObjectCall = Expression.Call
        (
            null,
            createMethodInfo,
            nestedSelectorParameter,
            constantMappingProperties
        );

        //For example: Product
        var entityParameter = Expression.Parameter(typeof(TEntity), typeof(TEntity).Name);
        //For example: Product.Reviews
        var entityNestedMember = entityParameter.ToMemberExpression(mappingEntry.From!);

        //For example: Review => Create(Review, MappingEntries)
        LambdaExpression lambdaExpression = Expression.Lambda(createDataTransferObjectCall, false, nestedSelectorParameter);

        var selectMethodInfo = typeof(Enumerable)
            .GetTypeInfo().GetDeclaredMethods(nameof(Enumerable.Select))
            .First()
            .MakeGenericMethod(nestedEntityType, typeof(DataTransferObject));

        //Product.Reviews.Select(Review => Create(Review, MappingEntries))
        var selectorCall = Expression.Call
        (
            null,
            selectMethodInfo,
            entityNestedMember,
            lambdaExpression
        );

        //return selectorCall;

        var asFunc = lambdaExpression.Compile();

        var result = selectorCall.Method.Invoke(null, [nestedEntityOrCollectionOfEntities, asFunc])!;
        return result;
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