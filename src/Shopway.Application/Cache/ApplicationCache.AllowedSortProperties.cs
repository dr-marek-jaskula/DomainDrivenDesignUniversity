using Shopway.Domain.Abstractions.Common;
using System.Collections.ObjectModel;

namespace Shopway.Application.Cache;

public static partial class ApplicationCache
{
    public static readonly ReadOnlyDictionary<Type, IReadOnlyCollection<string>> AllowedSortPropertiesCache;

    private static ReadOnlyDictionary<Type, IReadOnlyCollection<string>> CreateAllowedSortPropertiesCache()
    {
        Dictionary<Type, IReadOnlyCollection<string>> allowedSortPropertiesCache = new();

        var dynamicSortByTypes = Persistence.AssemblyReference.Assembly
            .GetTypes()
            .Where(type => type.GetInterface(nameof(IDynamicSortBy)) is not null)
            .ToArray();

        foreach (var type in dynamicSortByTypes)
        {
            var typeAllowedFilterProperties = type!.GetProperty(nameof(IDynamicSortBy.AllowedSortProperties))!.GetValue(null) as IReadOnlyCollection<string>;
            allowedSortPropertiesCache.TryAdd(type, typeAllowedFilterProperties!);
        }

        return allowedSortPropertiesCache.AsReadOnly();
    }
}