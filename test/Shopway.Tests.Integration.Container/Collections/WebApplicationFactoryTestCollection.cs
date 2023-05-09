using static Shopway.Tests.Integration.Constants.CollectionNames;

namespace Shopway.Tests.Integration.Collections;

[CollectionDefinition(WebApplicationFactoryCollection)]
public sealed class WebApplicationFactoryTestCollection
    : ICollectionFixture<ShopwayApiFactory> //This will ensure that one container will be created per collection
{
}