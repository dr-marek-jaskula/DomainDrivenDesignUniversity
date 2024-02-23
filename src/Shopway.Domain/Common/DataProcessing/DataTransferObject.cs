using Shopway.Domain.Common.BaseTypes.Abstractions;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Shopway.Domain.Common.DataProcessing;

public class DataTransferObject : IDictionary<string, string>
{
    protected readonly Dictionary<string, string> _dictionary = [];

    public DataTransferObject()
    {
    }

    protected DataTransferObject(DataTransferObject dataTransferObject)
    {
        _dictionary = dataTransferObject._dictionary;
    }

    public static DataTransferObject Create<TType>(TType entity, IList<string> properties)
        where TType : class, IEntity
    {
        var dictionaryResponse = new DataTransferObject();

        foreach (var property in properties)
        {
            var value = typeof(TType).GetProperty(property).GetValue(entity).ToString()!;
            dictionaryResponse.Add(property, value);
        }

        return dictionaryResponse;
    }

    public string this[string key] { get => _dictionary[key]; set => _dictionary[key] = value; }

    public ICollection<string> Keys => _dictionary.Keys;

    public ICollection<string> Values => _dictionary.Values;

    public int Count => _dictionary.Count;

    public bool IsReadOnly => false;

    public void Add(string key, string value)
    {
        _dictionary.Add(key, value);
    }

    public void Add(KeyValuePair<string, string> item)
    {
        _dictionary[item.Key] = item.Value;
    }

    public DataTransferObject AddIf(bool condition, string key, string value)
    {
        if (condition is false)
        {
            return this;
        }

        Add(key, value);
        return this;
    }

    public void Clear()
    {
        _dictionary.Clear();
    }

    public bool Contains(KeyValuePair<string, string> item)
    {
        return _dictionary.Contains(item);
    }

    public bool ContainsKey(string key)
    {
        return _dictionary.ContainsKey(key);
    }

    public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        return _dictionary.GetEnumerator();
    }

    public bool Remove(string key)
    {
        return _dictionary.Remove(key);
    }

    public bool Remove(KeyValuePair<string, string> item)
    {
        if (_dictionary.Contains(item))
        {
            return _dictionary.Remove(item.Key);
        }

        return false;
    }

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out string value)
    {
        return _dictionary.TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _dictionary.GetEnumerator();
    }
}