namespace Shopway.Domain.Common.BaseTypes.Abstractions;

/// <summary>
/// We use the Auditable pattern to set the "CreatedOn", "CreatedBy" and "UpdatedOn", "UpdatedBy" properties when saving changes by UnitOfWork.
/// </summary>
public interface IAuditable
{
    DateTimeOffset CreatedOn { get; set; }
    DateTimeOffset? UpdatedOn { get; set; }
    string CreatedBy { get; set; }
    string? UpdatedBy { get; set; }
}