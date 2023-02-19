using Shopway.Domain.BaseTypes;
using Shopway.Domain.Errors;
using Shopway.Domain.Results;
using Shopway.Domain.Utilities;
using static Shopway.Domain.Errors.Domain.DomainErrors;
using static Shopway.Domain.Utilities.ListUtilities;

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

    public static ValidationResult<StoryPoints> Create(int storyPoints)
    {
        var errors = Validate(storyPoints);
        return errors.CreateValidationResult(() => new StoryPoints(storyPoints));
    }

    public static List<Error> Validate(int storyPoints)
    {
        var errors = Empty<Error>();

        if (storyPoints < MinStoryPoints)
        {
            errors.Add(PriorityError.TooLow);
        }

        if (storyPoints > MaxStoryPoints)
        {
            errors.Add(PriorityError.TooHigh);
        }

        return errors;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

