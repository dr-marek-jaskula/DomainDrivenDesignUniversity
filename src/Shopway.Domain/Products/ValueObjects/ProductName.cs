using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using static Shopway.Domain.Common.Utilities.ListUtilities;

namespace Shopway.Domain.Products.ValueObjects;

public sealed record class ProductName : ValueObject<string>
{
    public const int MaxLength = 50;

    public static readonly Error Empty = Error.New(
        $"{nameof(ProductName)}.{nameof(Empty)}",
        $"{nameof(ProductName)} is empty.");

    public static readonly Error TooLong = Error.New(
        $"{nameof(ProductName)}.{nameof(TooLong)}",
        $"{nameof(ProductName)} must be at most {MaxLength} characters long.");

    public static readonly Error ContainsIllegalCharacter = Error.New(
        $"{nameof(ProductName)}.{nameof(ContainsIllegalCharacter)}",
        $"{nameof(ProductName)} contains illegal character.");

    private ProductName(string productName) : base(productName)
    {
    }

    public static ValidationResult<ProductName> Create(string productName)
    {
        var errors = Validate(productName);
        return errors.CreateValidationResult(() => new ProductName(productName));
    }

    public static IList<Error> Validate(string productName)
    {
        return EmptyList<Error>()
            .If(productName.IsNullOrEmptyOrWhiteSpace(), Empty)
            .If(productName.Length > MaxLength, TooLong)
            .If(productName.ContainsIllegalCharacter(), ContainsIllegalCharacter);
    }
}
