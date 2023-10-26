using Shopway.Domain.Abstractions.Common;
using System.Collections.ObjectModel;

namespace Shopway.Application.Cache;

public static partial class ApplicationCache
{
    public static readonly ReadOnlyDictionary<Type, IReadOnlyCollection<string>> AllowedFilterOperationsCache;

    private static ReadOnlyDictionary<Type, IReadOnlyCollection<string>> CreateAllowedFilterOperationsCache()
    {
        Dictionary<Type, IReadOnlyCollection<string>> allowedFilterOperations = new();

        var dynamicFilterTypes = Persistence.AssemblyReference.Assembly
            .GetTypes()
            .Where(type => type.GetInterface(nameof(IDynamicFilter)) is not null)
            .ToArray();

        foreach (var type in dynamicFilterTypes)
        {
            var typeAllowedFilterOperations = type!.GetProperty(nameof(IDynamicFilter.AllowedFilterOperations))!.GetValue(null) as IReadOnlyCollection<string>;
            allowedFilterOperations.TryAdd(type, typeAllowedFilterOperations!);
        }

        return allowedFilterOperations.AsReadOnly();
    }
}