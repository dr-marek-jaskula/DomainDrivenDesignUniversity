using static Shopway.Tests.Integration.Constants.Constants;
using static Shopway.Tests.Integration.Constants.Constants.CollectionName;

namespace Shopway.Tests.Integration.Persistence;

[Collection(DatabaseCollection)]
[Trait(nameof(IntegrationTest), IntegrationTest.CleanDatabase)]
public sealed class CleanTestData : IAsyncLifetime
{
    private readonly DatabaseFixture _fixture;

    public CleanTestData(DatabaseFixture databaseFixture)
    {
        _fixture = databaseFixture;
    }

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