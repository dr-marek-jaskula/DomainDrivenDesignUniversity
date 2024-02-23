using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Domain.Common.Utilities;
using System.Collections.Frozen;

namespace Shopway.Application.Cache;

public static partial class ApplicationCache
{
    public static readonly FrozenDictionary<Type, IReadOnlyCollection<string>> AllowedMappingPropertiesCache;

    private static FrozenDictionary<Type, IReadOnlyCollection<string>> CreateAllowedMappingPropertiesCache()
    {
        Dictionary<Type, IReadOnlyCollection<string>> allowedMappingPropertiesCache = [];

        var dynamicMappingTypes = Domain.AssemblyReference.Assembly
            .GetTypesWithAnyMatchingInterface(i => i.Name.Contains(nameof(IDynamicMapping)))
            .Where(type => type.IsInterface is false);

        foreach (var type in dynamicMappingTypes)
        {
            var typeAllowedMappingProperties = type!.GetProperty(nameof(IDynamicMapping.AllowedProperties))
                !.GetValue(null) as IReadOnlyCollection<string>;
            allowedMappingPropertiesCache.TryAdd(type, typeAllowedMappingProperties!);
        }

        return allowedMappingPropertiesCache.ToFrozenDictionary();
    }
}