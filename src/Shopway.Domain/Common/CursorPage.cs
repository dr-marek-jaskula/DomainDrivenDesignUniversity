using Shopway.Domain.Abstractions.Common;

namespace Shopway.Domain.Common;

public sealed class CursorPage : ICursorPage
{
    public required int PageSize { get; init; }
    public required Ulid Cursor { get; init; }
}