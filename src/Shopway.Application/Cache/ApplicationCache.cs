namespace Shopway.Application.Cache;

public static partial class ApplicationCache
{
    public static bool SeedCache { get; set; }

    static ApplicationCache()
    {
        SeedCache = true;
        ValidationCache = CreateValidationCache();
        AllowedFilterPropertiesCache = CreateAllowedFilterPropertiesCache();
        AllowedFilterOperationsCache = CreateAllowedFilterOperationsCache();
        AllowedSortPropertiesCache = CreateAllowedSortPropertiesCache();
    }
}