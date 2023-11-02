using Shopway.Tests.Integration.Persistence;
using static Shopway.Tests.Integration.Constants.Constants.CollectionName;

namespace Shopway.Tests.Integration.Collections;

[CollectionDefinition(DatabaseCollection)]
public sealed class DatabaseFixtureCollection
    : ICollectionFixture<DatabaseFixture>
{

}
