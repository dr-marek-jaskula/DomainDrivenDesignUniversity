using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Errors.Domain;

public static partial class DomainErrors
{
    public static class StoryPointsError
    {
        public static readonly Error TooLow = new(
            $"{nameof(StoryPoints)}.{nameof(TooLow)}",
            $"{nameof(StoryPoints)} must be at least {StoryPoints.MinStoryPoints}");

        public static readonly Error TooHigh = new(
            $"{nameof(StoryPoints)}.{nameof(TooHigh)}",
            $"{nameof(StoryPoints)} must be at most {StoryPoints.MaxStoryPoints}");
    }
}