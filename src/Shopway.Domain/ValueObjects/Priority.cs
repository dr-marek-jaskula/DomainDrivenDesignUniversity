using Shopway.Domain.BaseTypes;
using Shopway.Domain.Errors;
using Shopway.Domain.Results;
using Shopway.Domain.Utilities;
using static Shopway.Domain.Errors.Domain.DomainErrors;
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
        return errors.CreateValidationResult(() => new Priority(priority));
    }

    public static List<Error> Validate(int priority)
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

