using Shopway.Tests.Integration.Persistance;
using static Shopway.Tests.Integration.Collections.CollectionNames;

namespace Shopway.Tests.Integration.Collections;

[CollectionDefinition(Product_Controller_Collection)]
public sealed class ProductControllerTestCollection : ICollectionFixture<DatabaseFixture>
{
    //DatabaseFixture will be shared across all collection
}