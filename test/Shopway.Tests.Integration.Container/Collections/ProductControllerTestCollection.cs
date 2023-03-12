using static Shopway.Tests.Integration.Constants.CollectionNames;

namespace Shopway.Tests.Integration.Collections;

[CollectionDefinition(ProductControllerCollection)]
public sealed class ProductControllerTestCollection
    : ICollectionFixture<ShopwayApiFactory> //This will ensure that one container will be created per collection
{
}