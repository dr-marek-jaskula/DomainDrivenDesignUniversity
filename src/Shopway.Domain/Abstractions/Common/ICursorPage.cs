namespace Shopway.Domain.Abstractions.Common;

public interface ICursorPage : IPage
{
    Ulid Cursor { get; init; }
}