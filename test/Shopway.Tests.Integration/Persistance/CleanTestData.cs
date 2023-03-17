using static Shopway.Tests.Integration.Constants.CollectionNames;

namespace Shopway.Tests.Integration.Persistance;

[Collection(DatabaseCollection)]
public sealed class CleanTestData : IAsyncLifetime
{
    private readonly DatabaseFixture _fixture;

    public CleanTestData(DatabaseFixture databaseFixture)
    {
        _fixture = databaseFixture;
    }

    [Fact(Skip = "Only for cleaning the database manually")]
    public async Task CleanDatabaseFromTestData()
    {
        await _fixture.DataGenerator.CleanDatabaseFromTestData();
    }

    public async Task DisposeAsync()
    {
        await _fixture.DisposeAsync();
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }
}