using Shopway.Domain.Utilities;
using System.Collections.ObjectModel;
using Shopway.Domain.Abstractions.Common;

namespace Shopway.Application.Cache;

public static partial class ApplicationCache
{
    public static readonly ReadOnlyDictionary<Type, IReadOnlyCollection<string>> AllowedFilterOperationsCache;

    private static ReadOnlyDictionary<Type, IReadOnlyCollection<string>> CreateAllowedFilterOperationsCache()
    {
        Dictionary<Type, IReadOnlyCollection<string>> allowedFilterOperations = new();

        var dynamicFilterTypes = Persistence.AssemblyReference.Assembly
            .GetTypesWithAnyMatchingInterface(i => i.Name.Contains(nameof(IDynamicFilter)));

        foreach (var type in dynamicFilterTypes)
        {
            var typeAllowedFilterOperations = type!.GetProperty(nameof(IDynamicFilter.AllowedFilterOperations))
                !.GetValue(null) as IReadOnlyCollection<string>;
            allowedFilterOperations.TryAdd(type, typeAllowedFilterOperations!);
        }

        return allowedFilterOperations.AsReadOnly();
    }
}