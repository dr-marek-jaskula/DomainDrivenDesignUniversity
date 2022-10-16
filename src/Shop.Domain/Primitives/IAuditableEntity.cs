namespace Shopway.Domain.Primitives;

//We use the Auditable pattern to set the "CreatedOn" and "UpdatedOn" properties when saving changes.
//For this to happen we can use Interceptor pattern or override the SaveChanges (and SaveChangesAsync) methods
//The preferred way: use the "Interceptor" pattern
public interface IAuditableEntity
{
    DateTimeOffset CreatedOn { get; set; }

    DateTimeOffset? UpdatedOn { get; set; }
}
