namespace Shopway.Persistence.Cache;

public static partial class PersistenceCache
{
    public static bool SeedCache { get; set; }

    static PersistenceCache()
    {
        SeedCache = true;
        ValidationCache = CreateValidationCache();
    }
}