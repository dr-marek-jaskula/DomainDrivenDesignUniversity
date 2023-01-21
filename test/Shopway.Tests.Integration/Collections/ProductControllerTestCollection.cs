using RestSharp;
using static Shopway.Tests.Integration.Collections.CollectionNames;

namespace Shopway.Tests.Integration.Collections;

[CollectionDefinition(Product_Controller_Collection)]
public sealed class ProductControllerTestCollection : ICollectionFixture<RestClient>
{

}