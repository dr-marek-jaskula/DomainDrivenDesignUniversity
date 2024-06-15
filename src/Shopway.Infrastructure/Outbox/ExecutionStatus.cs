namespace Shopway.Infrastructure.Outbox;

public enum ExecutionStatus
{
    InProgress = 0,
    Failure = 1,
    Success = 2
}
