using Shopway.Domain.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Common.BaseTypes;
using static Shopway.Domain.Errors.Domain.DomainErrors;
using static Shopway.Domain.Common.Utilities.ListUtilities;
using Shopway.Domain.Common.Errors;

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
