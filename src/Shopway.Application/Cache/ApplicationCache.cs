namespace Shopway.Application.Cache;

public static partial class ApplicationCache
{
    public static bool SeedCache { get; set; }

    static ApplicationCache()
    {
        SeedCache = true;
        AllowedSortPropertiesCache = CreateAllowedSortPropertiesCache();
        AllowedFilterPropertiesCache = CreateAllowedFilterPropertiesCache();
        AllowedFilterOperationsCache = CreateAllowedFilterOperationsCache();
        AllowedMappingPropertiesCache = CreateAllowedMappingPropertiesCache();
    }
}