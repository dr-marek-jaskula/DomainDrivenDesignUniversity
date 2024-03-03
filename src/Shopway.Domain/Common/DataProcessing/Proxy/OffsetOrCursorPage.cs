using Shopway.Domain.Common.DataProcessing.Abstractions;

namespace Shopway.Domain.Common.DataProcessing.Proxy;

public sealed class OffsetOrCursorPage : IPage
{
    public required int PageSize { get; init; }
    public int? PageNumber { get; init; }
    public Ulid? Cursor { get; init; }

    public Type GetPageType()
    {
        if (Cursor is null)
        {
            return typeof(OffsetPage);
        }

        return typeof(CursorPage);
    }

    public OffsetPage ToOffsetPage()
    {
        return new OffsetPage()
        {
            PageSize = PageSize,
            PageNumber = (int)PageNumber!
        };
    }

    public CursorPage ToCursorPage()
    {
        return new CursorPage()
        {
            PageSize = PageSize,
            Cursor = (Ulid)Cursor!
        };
    }
}