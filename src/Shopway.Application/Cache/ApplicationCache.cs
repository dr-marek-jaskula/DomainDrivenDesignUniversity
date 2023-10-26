namespace Shopway.Application.Cache;

public static partial class ApplicationCache
{
    static ApplicationCache()
    {
        ValidationCache = CreateValidationCache();
        AllowedFilterPropertiesCache = CreateAllowedFilterPropertiesCache();
        AllowedFilterOperationsCache = CreateAllowedFilterOperationsCache();
        AllowedSortPropertiesCache = CreateAllowedSortPropertiesCache();
    }
}