namespace Shopway.Domain.Abstractions;

//We use the Auditable pattern to set the "CreatedOn" and "UpdatedOn" properties when saving changes.
//For this to happen we can use Interceptor pattern or override the SaveChanges (and SaveChangesAsync) methods
//The preferred way: override SaveChanges
public interface IAuditable
{
    DateTimeOffset CreatedOn { get; set; }
    DateTimeOffset? UpdatedOn { get; set; }
    string CreatedBy { get; set; }
    string? UpdatedBy { get; set; }
}