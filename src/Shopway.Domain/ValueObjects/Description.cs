using Shopway.Domain.Errors;
using Shopway.Domain.Primitives;
using Shopway.Domain.Results;
using static Shopway.Domain.Errors.DomainErrors;

namespace Shopway.Domain.ValueObjects;

public sealed class Description : ValueObject
{
    public const int MaxLength = 600;

    private Description(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Description> Create(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            return Result.Failure<Description>(DescriptionError.Empty);
        }

        if (description.Length > MaxLength)
        {
            return Result.Failure<Description>(DescriptionError.TooLong);
        }

        return new Description(description);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
