using Shopway.Tests.Integration.Configurations;
using static Shopway.Tests.Integration.Constants.CollectionNames;

namespace Shopway.Tests.Integration.Collections;

[CollectionDefinition(ProductControllerCollection)]
public sealed class ProductControllerTestCollection
    : ICollectionFixture<ShopwayApiFactory>,
      ICollectionFixture<IntegrationTestsUrlOptions>,
      ICollectionFixture<ApiKeyTestOptions>
{
}