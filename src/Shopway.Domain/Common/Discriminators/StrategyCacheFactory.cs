using System.Collections.Frozen;
using System.Reflection;

namespace Shopway.Domain.Common.Discriminators;

public sealed class StrategyCacheFactory<DelegateType>
    where DelegateType : notnull, Delegate
{
    private StrategyCacheFactory()
    {
    }

    public static FrozenDictionary<string, DelegateType> CreateFor<TType, AttributeType>()
        where AttributeType : StrategyAttribute
    {
        var strategies = typeof(TType)
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
            .Where(method => method.GetCustomAttribute<AttributeType>() is not null)
            .Select(x => x.CreateDelegate<DelegateType>());

        return CreateFor<AttributeType>(strategies);
    }

    public static FrozenDictionary<string, DelegateType> CreateFor<AttributeType>(IEnumerable<DelegateType> dictionaryValues)
        where AttributeType : StrategyAttribute
    {
        Dictionary<string, DelegateType> cache = [];

        foreach (var @delegate in dictionaryValues)
        {
            AddToCache<AttributeType>(cache, @delegate);
        }

        return cache.ToFrozenDictionary();
    }

    private static void AddToCache<AttributeType>(Dictionary<string, DelegateType> cache, DelegateType @delegate)
        where AttributeType : StrategyAttribute
    {
        var strategyAttribute = @delegate
            .GetMethodInfo()
            .GetCustomAttribute<AttributeType>()
            ?? throw new InvalidOperationException($"Each delegate must have custom attribute 'Strategy'.");

        if (cache.TryAdd(strategyAttribute.GetKey(), @delegate) is false)
        {
            throw new InvalidOperationException($"Duplicated 'Key' for '{strategyAttribute.GetKey()}'.");
        }
    }
}

public sealed class StrategyCacheFactory<DiscriminatorType, DelegateType>
    where DiscriminatorType : Discriminator
    where DelegateType : notnull, Delegate
{
    private StrategyCacheFactory()
    {
    }

    public static FrozenDictionary<DiscriminatorType, DelegateType> CreateFor<TType, AttributeType>()
        where AttributeType : StrategyAttribute<DiscriminatorType>
    {
        var strategies = typeof(TType)
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
            .Where(method => method.GetCustomAttribute<AttributeType>() is not null)
            .Select(x => x.CreateDelegate<DelegateType>());

        return CreateFor<AttributeType>(strategies);
    }

    public static FrozenDictionary<DiscriminatorType, DelegateType> CreateFor<AttributeType>(IEnumerable<DelegateType> dictionaryValues)
        where AttributeType : StrategyAttribute<DiscriminatorType>
    {
        Dictionary<DiscriminatorType, DelegateType> cache = [];

        foreach (var @delegate in dictionaryValues)
        {
            AddToCache<AttributeType>(cache, @delegate);
        }

        return cache.ToFrozenDictionary();
    }

    private static void AddToCache<AttributeType>(Dictionary<DiscriminatorType, DelegateType> cache, DelegateType @delegate)
        where AttributeType : StrategyAttribute<DiscriminatorType>
    {
        var strategyAttribute = @delegate
            .GetMethodInfo()
            .GetCustomAttribute<AttributeType>()
            ?? throw new InvalidOperationException($"Each delegate must have custom attribute 'Strategy' of type {typeof(DiscriminatorType)}.");

        if (cache.TryAdd(strategyAttribute.ToDiscriminator(), @delegate) is false)
        {
            throw new InvalidOperationException($"Duplicated 'Discriminator' for '{strategyAttribute.ToDiscriminator()}'.");
        }
    }
}
