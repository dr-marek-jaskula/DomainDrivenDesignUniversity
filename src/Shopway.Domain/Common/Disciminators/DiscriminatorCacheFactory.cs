using System.Collections.Frozen;
using System.Reflection;

namespace Shopway.Domain.Common.Disciminators;

public sealed class DiscriminatorCacheFactory<DiscriminatorType, DelegateType>
    where DiscriminatorType : Discriminator
    where DelegateType : notnull, Delegate
{
    private DiscriminatorCacheFactory()
    {
    }

    public static FrozenDictionary<DiscriminatorType, DelegateType> CreateFor<AttributeType>(IEnumerable<DelegateType> dictionaryValues)
        where AttributeType : DiscriminatorAttribute<DiscriminatorType>
    {
        Dictionary<DiscriminatorType, DelegateType> cache = [];

        foreach (var @delegate in dictionaryValues)
        {
            AddToCache<AttributeType>(cache, @delegate);
        }

        return cache.ToFrozenDictionary();
    }

    private static void AddToCache<AttributeType>(Dictionary<DiscriminatorType, DelegateType> cache, DelegateType @delegate)
        where AttributeType : DiscriminatorAttribute<DiscriminatorType>
    {
        var discriminator = @delegate
            .GetMethodInfo()
            .GetCustomAttribute<AttributeType>()
            ?? throw new InvalidOperationException($"Each delegate must have custom attribute 'Discriminator' of type {typeof(DiscriminatorType)}.");

        if (cache.TryAdd(discriminator.ToDiscriminator(), @delegate) is false)
        {
            throw new InvalidOperationException($"Duplicated 'Discriminator' for '{discriminator.ToDiscriminator()}'.");
        }
    }
}