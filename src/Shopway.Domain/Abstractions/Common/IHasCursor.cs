namespace Shopway.Domain.Abstractions.Common;

public interface IHasCursor
{
    public Ulid Id { get; }
}