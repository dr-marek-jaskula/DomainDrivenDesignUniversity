using Microsoft.EntityFrameworkCore;
using Shopway.Persistence.Framework;
using Shopway.Persistence.Repositories;
using Shopway.Tests.Integration.Container.Api;
using ZiggyCreatures.Caching.Fusion;

namespace Shopway.Tests.Integration.Persistence;

public sealed class DatabaseFixture : IDisposable
{
    private readonly ShopwayDbContext _context;
    private readonly TestDataGenerator _testDataGenerator;
    private readonly TimeProvider _timeProvider;

    public DatabaseFixture(string connectionString)
    {
        var factory = new ShopwayDbContextFactory();
        _context = factory.CreateDbContext([connectionString]);
        _context.Database.Migrate();
        _timeProvider = TimeProvider.System;

        var testContext = new TestUserContextService();
        var outboxRepository = new OutboxRepository(_context, _timeProvider);
        var fusionCache = new FusionCache(new FusionCacheOptions());
        var unitOfWork = new UnitOfWork<ShopwayDbContext>(_context, testContext, outboxRepository, fusionCache);
        _testDataGenerator = new TestDataGenerator(unitOfWork);
    }

    /// <summary>
    /// Use to generate test data
    /// </summary>
    public TestDataGenerator DataGenerator => _testDataGenerator;

    /// <summary>
    /// Database context
    /// </summary>
    public ShopwayDbContext Context => _context;

    public void Dispose()
    {
        Context.Dispose();
        GC.SuppressFinalize(this);
    }
}
