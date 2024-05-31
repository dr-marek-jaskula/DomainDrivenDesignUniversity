using Shopway.Application.Abstractions;
using Shopway.Domain.Common.DataProcessing;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Shopway.Application.Features;

public class DataTransferObjectResponse(DataTransferObject dataTransferObject) : IDictionary<string, object>, IResponse
{
    protected readonly Dictionary<string, object?> _dictionary = dataTransferObject.Dictionary;

    public static DataTransferObjectResponse From(DataTransferObject dataTransferObject)
    {
        return new DataTransferObjectResponse(dataTransferObject);
    }

    public Dictionary<string, object?> Dictionary => _dictionary;

    public object this[string key] { get => _dictionary[key]!; set => _dictionary[key] = value; }

    public ICollection<string> Keys => _dictionary.Keys;

    public ICollection<object> Values => _dictionary.Values!;

    public int Count => _dictionary.Count;

    public bool IsReadOnly => false;

    public void Add(string key, object? value)
    {
        _dictionary.Add(key, value);
    }

    public void Add(KeyValuePair<string, object> item)
    {
        _dictionary[item.Key] = item.Value;
    }

    public void Clear()
    {
        _dictionary.Clear();
    }

    public bool Contains(KeyValuePair<string, object> item)
    {
        return _dictionary!.Contains(item);
    }

    public bool ContainsKey(string key)
    {
        return _dictionary.ContainsKey(key);
    }

    public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
    {
        ((IDictionary<string, object?>)_dictionary).CopyTo(array!, arrayIndex);
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
        if (_dictionary!.Contains(item))
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
