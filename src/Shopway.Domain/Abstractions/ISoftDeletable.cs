namespace Shopway.Domain.Abstractions;

public interface ISoftDeletable
{
    DateTimeOffset? SoftDeletedOn { get; set; }
    bool SoftDeleted { get; set; }

    abstract void SoftDelete();
}