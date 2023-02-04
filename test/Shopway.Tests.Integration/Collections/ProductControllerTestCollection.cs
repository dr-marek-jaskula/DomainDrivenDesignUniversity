using Shopway.Tests.Integration.Persistance;
using static Shopway.Tests.Integration.Constants.CollectionNames;

namespace Shopway.Tests.Integration.Collections;

[CollectionDefinition(ProductControllerCollection)]
public sealed class ProductControllerTestCollection 
    : ICollectionFixture<DatabaseFixture>, 
      ICollectionFixture<DependencyInjectionContainerTestFixture>
{
    //DatabaseFixture and DependencyInjectionContainerTestFixture will be shared across all classed with this collection attribute
}