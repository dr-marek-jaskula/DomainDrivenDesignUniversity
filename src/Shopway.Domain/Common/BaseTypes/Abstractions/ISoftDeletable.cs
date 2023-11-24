namespace Shopway.Domain.Common.BaseTypes.Abstractions;

public interface ISoftDeletable
{
    DateTimeOffset? SoftDeletedOn { get; set; }
    bool SoftDeleted { get; set; }

    abstract void SoftDelete();
}