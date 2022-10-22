using Shopway.Domain.Errors;
using Shopway.Domain.Primitives;
using Shopway.Domain.Results;

namespace Shopway.Domain.ValueObjects;

public sealed class StoryPoints : ValueObject
{
    public const int MaxStoryPoints = 10;
    public const int MinStoryPoints = 1;

    public int Value { get; }

    private StoryPoints(int value)
    {
        Value = value;
    }

    public static Result<StoryPoints> Create(int storyPoints)
    {
        if (storyPoints < MinStoryPoints)
        {
            return Result.Failure<StoryPoints>(DomainErrors.PriorityError.TooHigh);
        }

        if (storyPoints > MaxStoryPoints)
        {
            return Result.Failure<StoryPoints>(DomainErrors.PriorityError.TooHigh);
        }

        return new StoryPoints(storyPoints);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

