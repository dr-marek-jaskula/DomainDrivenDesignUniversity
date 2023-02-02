using Shopway.Persistence.Framework;

namespace Shopway.Tests.Integration.Persistance;

public sealed class DatabaseFixture : IDisposable
{
    private const string _testConnection = "TestConnection";
    private readonly ShopwayDbContext _context;

    public DatabaseFixture()
    {
        var factory = new ShopwayDbContextFactory();
        _context = factory.CreateDbContext(new[] { _testConnection });
        //_context.Database.EnsureDeleted();
        //_context.Database.Migrate();

        DataGenerator = new TestDataGenerator(Context);
    }

    public TestDataGenerator DataGenerator { get; }
    public ShopwayDbContext Context => _context;

    public void Dispose()
    {
        Context.Dispose();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task CleanDatabase()
    {
        await DataGenerator.CleanupDatabase();
    }
}
