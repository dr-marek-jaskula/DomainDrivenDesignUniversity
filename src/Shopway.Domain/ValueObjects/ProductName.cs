using Shopway.Domain.Errors;
using Shopway.Domain.Utilities;
using Shopway.Domain.Primitives;
using Shopway.Domain.Results;

namespace Shopway.Domain.ValueObjects;

public sealed class ProductName : ValueObject
{
    public const int MaxLength = 50;

    public string Value { get; }

    private ProductName(string value)
    {
        Value = value;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static Result<ProductName> Create(string productName)
    {
        if (string.IsNullOrWhiteSpace(productName))
        {
            return Result.Failure<ProductName>(DomainErrors.ProductNameError.Empty);
        }

        if (productName.Length > MaxLength)
        {
            return Result.Failure<ProductName>(DomainErrors.ProductNameError.TooLong);
        }

        if (productName.ContainsIllegalCharacter())
        {
            return Result.Failure<ProductName>(DomainErrors.ProductNameError.ContainsIllegalCharacter);
        }

        return new ProductName(productName);
    }
}