using Shopway.Domain.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Common.BaseTypes;
using static Shopway.Domain.Errors.Domain.DomainErrors;
using static Shopway.Domain.Common.Utilities.ListUtilities;

namespace Shopway.Domain.Products.ValueObjects;

public sealed class ProductName : ValueObject
{
    public const int MaxLength = 50;

    public new string Value { get; }

    private ProductName(string value)
    {
        Value = value;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static ValidationResult<ProductName> Create(string productName)
    {
        var errors = Validate(productName);
        return errors.CreateValidationResult(() => new ProductName(productName));
    }

    public static IList<Error> Validate(string productName)
    {
        return EmptyList<Error>()
            .If(productName.IsNullOrEmptyOrWhiteSpace(), ProductNameError.Empty)
            .If(productName.Length > MaxLength, ProductNameError.TooLong)
            .If(productName.ContainsIllegalCharacter(), ProductNameError.ContainsIllegalCharacter);
    }
}