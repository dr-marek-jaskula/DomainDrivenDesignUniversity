using Shopway.Domain.Errors;
using Shopway.Domain.Primitives;
using Shopway.Domain.Results;

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

    public static Result<Priority> Create(int priority)
    {
        if (priority < HighestPriority)
        {
            return Result.Failure<Priority>(DomainErrors.PriorityError.TooHigh);
        }

        if (priority > LowestPriority)
        {
            return Result.Failure<Priority>(DomainErrors.PriorityError.TooHigh);
        }

        return new Priority(priority);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

