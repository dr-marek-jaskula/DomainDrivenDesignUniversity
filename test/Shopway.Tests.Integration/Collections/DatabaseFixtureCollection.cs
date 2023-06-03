using Shopway.Tests.Integration.Persistence;
using static Shopway.Tests.Integration.Constants.CollectionNames;

namespace Shopway.Tests.Integration.Collections;

[CollectionDefinition(DatabaseCollection)]
public sealed class DatabaseFixtureCollection
    : ICollectionFixture<DatabaseFixture>
{

}
