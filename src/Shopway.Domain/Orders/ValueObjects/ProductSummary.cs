using Shopway.Domain.Errors;
using Shopway.Domain.Products;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Products.ValueObjects;
using static Shopway.Domain.Common.Utilities.ListUtilities;
using Shopway.Domain.Common.Errors;

namespace Shopway.Domain.Orders.ValueObjects;

public sealed class ProductSummary : ValueObject
{
    private ProductSummary(ProductId productId, string productName, string revision, decimal price, string uomCode)
    {
        ProductId = productId;
        ProductName = ProductName.Create(productName).Value;
        Revision = Revision.Create(revision).Value;
        Price = Price.Create(price).Value;
        UomCode = UomCode.Create(uomCode).Value;
    }

    //Empty constructor in this case is required by EF Core, because has a complex type as a parameter in the default constructor.
    private ProductSummary()
    {
    }

    public ProductId ProductId { get; private set; }
    public ProductName ProductName { get; private set; }
    public Revision Revision { get; private set; }
    public Price Price { get; private set; }
    public UomCode UomCode { get; private set; }

    public static ValidationResult<ProductSummary> Create(ProductId productId, string productName, string revision, decimal price, string uomCode)
    {
        var errors = Validate(productName, revision, price, uomCode);
        return errors.CreateValidationResult(() => new ProductSummary(productId, productName, revision, price, uomCode));
    }

    public static IList<Error> Validate(string productName, string revision, decimal price, string uomCode)
    {
        return EmptyList<Error>()
            .UseValidation(ProductName.Validate, productName)
            .UseValidation(Revision.Validate, revision)
            .UseValidation(Price.Validate, price)
            .UseValidation(UomCode.Validate, uomCode);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return ProductId;
        yield return ProductName;
        yield return Revision;
        yield return Price;
        yield return UomCode;
    }
}