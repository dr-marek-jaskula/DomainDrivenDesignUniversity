using static Shopway.Tests.Integration.Constants.Constants.CollectionName;

namespace Shopway.Tests.Integration.Persistence;

[Collection(DatabaseCollection)]
[IntegrationTest.CleanDatabase]
public sealed class CleanTestData(DatabaseFixture databaseFixture) : IAsyncLifetime
{
    private readonly DatabaseFixture _fixture = databaseFixture;

    [Fact(Skip = "Only for cleaning the database manually")]
    public async Task CleanDatabaseFromTestDataAsync()
    {
        await _fixture.DataGenerator.CleanDatabaseFromTestDataAsync();
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
