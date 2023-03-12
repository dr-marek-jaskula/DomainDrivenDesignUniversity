using Customers.Api.Tests.Integration;
using Shopway.Tests.Integration.Configurations;
using Shopway.Tests.Integration.Persistance;
using static Shopway.Tests.Integration.Constants.CollectionNames;

namespace Shopway.Tests.Integration.Collections;

[CollectionDefinition(ProductControllerCollection)]
public sealed class ProductControllerTestCollection
    : ICollectionFixture<ShopwayApiFactory>,
      ICollectionFixture<IntegrationTestsUrlOptions>,
      ICollectionFixture<ApiKeyTestOptions>
      //ICollectionFixture<DatabaseFixture>
{
}