using Shopway.Domain.Utilities;
using Shopway.Domain.Results;
using static Shopway.Domain.Errors.DomainErrors;
using Shopway.Domain.Abstractions.BaseTypes;

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
            return Result.Failure<ProductName>(ProductNameError.Empty);
        }

        if (productName.Length > MaxLength)
        {
            return Result.Failure<ProductName>(ProductNameError.TooLong);
        }

        if (productName.ContainsIllegalCharacter())
        {
            return Result.Failure<ProductName>(ProductNameError.ContainsIllegalCharacter);
        }

        return new ProductName(productName);
    }
}