using Shopway.Domain.BaseTypes;
using Shopway.Domain.Errors;
using Shopway.Domain.Results;
using static Shopway.Domain.Errors.DomainErrors;
using static Shopway.Domain.Utilities.ListUtilities;

namespace Shopway.Domain.ValueObjects;

public sealed class Priority : ValueObject
{
    public const int LowestPriority = 5;
    public const int HighestPriority = 0;

    public int Value { get; }

    private Priority(int value)
    {
        Value = value;
    }

    public static ValidationResult<Priority> Create(int priority)
    {
        var errors = Validate(priority);

        if (errors.Any())
        {
            return ValidationResult<Priority>.WithErrors(errors.ToArray());
        }

        return ValidationResult<Priority>.WithoutErrors(new Priority(priority));
    }

    private static List<Error> Validate(int priority)
    {
        var errors = Empty<Error>();

        if (priority < HighestPriority)
        {
            errors.Add(PriorityError.TooHigh);
        }

        if (priority > LowestPriority)
        {
            errors.Add(PriorityError.TooLow);
        }

        return errors;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

