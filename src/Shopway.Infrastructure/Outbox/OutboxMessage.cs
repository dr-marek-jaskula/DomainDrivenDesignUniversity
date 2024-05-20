namespace Shopway.Infrastructure.Outbox;

public sealed class OutboxMessage
{
    private const int InitialAttemptCount = 0;
    public const int InitialDelayInMinutes = 0;
    private static readonly int[] RetryDelaysInMinutes = [InitialDelayInMinutes, 1, 5, 15, 60];

    public required Ulid Id { get; set; }

    public required string Type { get; set; }

    public required string Content { get; set; }

    public required DateTimeOffset OccurredOn { get; set; }

    public ExecutionStatus ExecutionStatus { get; set; }

    public int AttemptCount { get; private set; } = InitialAttemptCount;
     
    public DateTimeOffset? NextProcessAttempt { get; set; }

    public DateTimeOffset? ProcessedOn { get; private set; }

    public string? Error { get; private set; }

    public void UpdatePostProcessProperties(DateTimeOffset? utcNow, string? error)
    {
        AttemptCount++;

        if (IsFailure(error))
        {
            UpdateWhenFailure(utcNow, error);
            return;
        }

        if (IsSuccess(error))
        {
            UpdateWhenSuccess(utcNow);
            return;
        }

        UpdateWhenInProgress(error);
    }

    private bool IsFailure(string? error)
    {
        return error is not null 
            && AttemptCount >= RetryDelaysInMinutes.Length;
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

    private void UpdateWhenInProgress(string? error)
    {
        if (NextProcessAttempt is null)
        {
            throw new ArgumentException(nameof(NextProcessAttempt));
        }

        Error = error;
        NextProcessAttempt = NextProcessAttempt.Value.AddMinutes(RetryDelaysInMinutes[AttemptCount]);
    }
}