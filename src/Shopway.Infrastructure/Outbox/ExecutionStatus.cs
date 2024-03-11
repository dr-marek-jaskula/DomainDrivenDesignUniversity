namespace Shopway.Infrastructure.Outbox;

public enum ExecutionStatus
{
    InProgress,
    Failure,
    Success
}