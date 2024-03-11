using Shopway.Infrastructure.Outbox;

namespace Shopway.Persistence.Outbox;

public sealed class OutboxMessage
{
    private const int MaxAttemptCount = 4;
    private static readonly int[] RetryDelaysInMinutes = [ 1, 5, 15, 60];

    public required Ulid Id { get; set; }

    public required string Type { get; set; }

    public required string Content { get; set; }

    public required DateTimeOffset OccurredOn { get; set; }

    public ExecutionStatus ExecutionStatus { get; set; }

    public int AttemptCount { get; set; }

    public DateTimeOffset? NextProcessAttempt { get; set; }

    public DateTimeOffset? ProcessedOn { get; private set; }

    public string? Error { get; private set; }

    public void UpdatePostProcessProperties(DateTimeOffset? utcNow, string? error)
    {
        if (IsFailure())
        {
            UpdateWhenFailure(utcNow, error);
            return;
        }

        if (IsSuccess(error))
        {
            UpdateWhenSuccess(utcNow);
            return;
        }

        UpdateWhenInProgress();
    }

    private bool IsFailure()
    {
        return AttemptCount >= MaxAttemptCount;
    }

    private static bool IsSuccess(string? error)
    {
        return error is null;
    }

    private void UpdateWhenFailure(DateTimeOffset? processedOn, string? error)
    {
        ExecutionStatus = ExecutionStatus.Failure;
        NextProcessAttempt = null;
        ProcessedOn = processedOn;
        Error = error;
    }

    private void UpdateWhenSuccess(DateTimeOffset? processedOn)
    {
        ExecutionStatus = ExecutionStatus.Success;
        NextProcessAttempt = null;
        ProcessedOn = processedOn;
        Error = null;
    }

    private void UpdateWhenInProgress()
    {
        if (NextProcessAttempt is null)
        {
            throw new ArgumentException(nameof(NextProcessAttempt));
        }

        AttemptCount++;
        NextProcessAttempt = NextProcessAttempt.Value.AddMinutes(RetryDelaysInMinutes[AttemptCount - 1]);
    }
}