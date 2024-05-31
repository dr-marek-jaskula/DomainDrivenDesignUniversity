using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Domain.Common.Utilities;
using System.Collections.Frozen;

namespace Shopway.Application.Cache;

public static partial class ApplicationCache
{
    public static readonly FrozenDictionary<Type, IReadOnlyCollection<string>> AllowedSortPropertiesCache;

    private static FrozenDictionary<Type, IReadOnlyCollection<string>> CreateAllowedSortPropertiesCache()
    {
        Dictionary<Type, IReadOnlyCollection<string>> allowedSortPropertiesCache = [];

        var dynamicSortByTypes = Domain.AssemblyReference.Assembly
            .GetTypesWithAnyMatchingInterface(i => i.Name.Contains(nameof(IDynamicSortBy)))
            .Where(type => type.IsInterface is false);

        foreach (var type in dynamicSortByTypes)
        {
            if (type.IsGenericType)
            {
                continue;
            }

            var typeAllowedSortByProperties = type!.GetProperty(nameof(IDynamicSortBy.AllowedSortProperties))
                !.GetValue(null) as IReadOnlyCollection<string>;
            allowedSortPropertiesCache.TryAdd(type, typeAllowedSortByProperties!);
        }

        return allowedSortPropertiesCache.ToFrozenDictionary();
    }
}
