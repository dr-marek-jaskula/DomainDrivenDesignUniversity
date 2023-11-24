using Shopway.Domain.Common.DataProcessing.Abstractions;

namespace Shopway.Domain.Common.DataProcessing;

public sealed class CursorPage : ICursorPage
{
    public required int PageSize { get; init; }
    public required Ulid Cursor { get; init; }
}