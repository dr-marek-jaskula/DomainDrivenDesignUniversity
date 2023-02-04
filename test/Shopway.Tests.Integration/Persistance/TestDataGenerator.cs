using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Domain.ValueObjects;
using Shopway.Persistence.Framework;
using Shopway.Tests.Integration.Abstractions;

namespace Shopway.Tests.Integration.Persistance;

/// <summary>
/// Contains methods to add entities to the database and utility methods for test data
/// </summary>
public sealed class TestDataGenerator : TestDataGeneratorBase
{
    public TestDataGenerator(IUnitOfWork<ShopwayDbContext> unitOfWork) 
        : base(unitOfWork)
    {
    }

    public async Task<ProductId> AddProductWithoutReviews
    (
        ProductName? productName = null,
        Revision? revision = null,
        Price? price = null,
        UomCode? uomCode = null
    )
    {
        productName ??= ProductName.Create(TestString(20)).Value;
        price ??= Price.Create(TestInt(1, 10)).Value;
        uomCode ??= UomCode.Create(UomCode.AllowedUomCodes.ElementAt(_random.Next(0, UomCode.AllowedUomCodes.Length - 1))).Value;
        revision ??= Revision.Create(TestString(2)).Value;

        var product = Product.Create
        (
            id: ProductId.New(),
            productName: productName,
            price: price,
            uomCode: uomCode,
            revision: revision
        );

        await AddEntity(product);

        return product
            .Id;
    }
}