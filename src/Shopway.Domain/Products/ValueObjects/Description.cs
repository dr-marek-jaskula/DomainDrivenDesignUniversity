using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Errors;
using static Shopway.Domain.Common.Utilities.ListUtilities;
using static Shopway.Domain.Products.Errors.DomainErrors;

namespace Shopway.Domain.Products.ValueObjects;

public sealed class Description : ValueObject
{
    public const int MaxLength = 600;

    private Description(string value)
    {
        Value = value;
    }

    public new string Value { get; }

    public static ValidationResult<Description> Create(string description)
    {
        var errors = Validate(description);
        return errors.CreateValidationResult(() => new Description(description));
    }

    public static IList<Error> Validate(string description)
    {
        return EmptyList<Error>()
            .If(description.IsNullOrEmptyOrWhiteSpace(), DescriptionError.Empty)
            .If(description.Length > MaxLength, DescriptionError.TooLong);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
