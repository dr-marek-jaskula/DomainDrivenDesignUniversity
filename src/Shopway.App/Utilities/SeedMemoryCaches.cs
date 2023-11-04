using Serilog;
using Shopway.Application.Cache;
using Shopway.Persistence.Cache;

namespace Shopway.App.Utilities;

public static class SeedMemoryCaches
{
    public static void Execute()
    {
        Log.Information("Seeding Application Layer Memory Cache: {SeedCache}", ApplicationCache.SeedCache);
        Log.Information("Seeding Persistence Layer Memory Cache: {SeedCache}", PersistenceCache.SeedCache);
    }
}