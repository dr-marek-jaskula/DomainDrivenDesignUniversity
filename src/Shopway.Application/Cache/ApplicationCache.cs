using Shopway.Domain.Results;

namespace Shopway.Application.Cache;

public static partial class ApplicationCache
{
    public static bool SeedCache { get; set; }

    static ApplicationCache()
    {
        SeedCache = true;
        Result.Success();
        ValidationResult.WithoutErrors();
        ValidationCache = CreateValidationCache();
        AllowedFilterPropertiesCache = CreateAllowedFilterPropertiesCache();
        AllowedFilterOperationsCache = CreateAllowedFilterOperationsCache();
        AllowedSortPropertiesCache = CreateAllowedSortPropertiesCache();
    }
}