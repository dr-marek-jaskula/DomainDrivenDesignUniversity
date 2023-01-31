using Shopway.Persistence.Framework;

namespace Shopway.Tests.Integration.Persistance;

public sealed class DatabaseFixture : IDisposable
{
    public readonly TestDataGenerator DataGenerator;
    public const string _testConnection = "TestConnection";

    public DatabaseFixture()
    {
        var factory = new ShopwayDbContextFactory();
        Context = factory.CreateDbContext(new[] { _testConnection });
        //Context.Database.EnsureDeleted();
        //Context.Database.Migrate();

        DataGenerator = new TestDataGenerator(Context);
    }

    public ShopwayDbContext Context { get; }

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
        await DataGenerator.ClearDatabase();
    }
}
