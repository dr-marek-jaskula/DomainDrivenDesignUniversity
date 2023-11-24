namespace Shopway.Domain.Common.DataProcessing.Abstractions;

public interface ICursorPage : IPage
{
    Ulid Cursor { get; init; }
}