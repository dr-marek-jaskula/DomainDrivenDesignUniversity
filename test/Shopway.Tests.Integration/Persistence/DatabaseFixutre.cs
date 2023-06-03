using Microsoft.EntityFrameworkCore;
using Shopway.Persistence.Framework;
using Shopway.Persistence.Outbox;
using ZiggyCreatures.Caching.Fusion;
using static Shopway.Persistence.Constants.ConnectionConstants;

namespace Shopway.Tests.Integration.Persistence;

public sealed class DatabaseFixture : IDisposable, IAsyncLifetime
{
    private readonly ShopwayDbContext _context;
    private readonly TestDataGenerator _testDataGenerator;

    public DatabaseFixture()
    {
        var factory = new ShopwayDbContextFactory();
        _context = factory.CreateDbContext(new[] { TestConnection });
        _context.Database.Migrate();

        var testContext = new TestContextService();
        var outboxRepository = new OutboxRepository(_context);
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

    public async Task DisposeAsync()
    {
        try
        {
            await DataGenerator.CleanDatabaseFromTestDataAsync();
        }
        catch
        {
            Console.WriteLine("CleanTestData.Integration.Api.Tests failed.");
        }
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }
}
