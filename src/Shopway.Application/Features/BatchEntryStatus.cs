namespace Shopway.Application.Features;

/// <summary>
/// Each response entry has its status. Error status overwrites other if at least one error for respective request occurs
/// </summary>
public enum BatchEntryStatus
{
    Inserted,
    Updated,
    Error
}