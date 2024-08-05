using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;

namespace Shopway.Domain.Common.DataProcessing.Proxy;

public sealed class OffsetOrCursorPage : IPage
{
    private static readonly Result _notOffsetOrCursorPageFailureResult = Error.InvalidArgument("Cursor or PageNumber must be provided.").ToResult();
    private static readonly Result _bothOffsetAndCursorPageFailureResult = Error.InvalidArgument("Both Cursor and PageNumber cannot be provided.").ToResult();

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

    public Result IsNotOffsetOrCursorPage()
    {
        if (Cursor is null && PageNumber is null)
        {
            return _notOffsetOrCursorPageFailureResult;
        }

        return Result.Success();
    }

    public Result IsBothOffsetAndCursorPage()
    {
        if (Cursor is not null && PageNumber is not null)
        {
            return _bothOffsetAndCursorPageFailureResult;
        }

        return Result.Success();
    }
}
