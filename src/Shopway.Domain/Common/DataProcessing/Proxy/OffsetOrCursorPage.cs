using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;

namespace Shopway.Domain.Common.DataProcessing.Proxy;

public sealed class OffsetOrCursorPage : IPage
{
    private const string NotCursorAndNotOffsetError = "Cursor or PageNumber must be provided.";
    private const string BothCursorAndOffsetError = "Both Cursor and PageNumber cannot be provided.";

    private static readonly Result _notOffsetOrCursorPageFailureResult = Error.InvalidArgument(NotCursorAndNotOffsetError).ToResult();
    private static readonly Result _bothOffsetAndCursorPageFailureResult = Error.InvalidArgument(BothCursorAndOffsetError).ToResult();

    private static readonly Result<string> _notOffsetOrCursorPageFailureStringResult = Error.InvalidArgument(NotCursorAndNotOffsetError).ToResult<string>();
    private static readonly Result<string> _bothOffsetAndCursorPageFailureStringResult = Error.InvalidArgument(BothCursorAndOffsetError).ToResult<string>();

    private static readonly Result<string> _offsetPageNameResult = Result.Success(nameof(OffsetPage));
    private static readonly Result<string> _cursorPageResult = Result.Success(nameof(CursorPage));

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

    public Result<string> GetPageName()
    {
        var pageIsNotOffsetOrCursorPageResult = IsNotOffsetOrCursorPage();

        if (pageIsNotOffsetOrCursorPageResult.IsFailure)
        {
            return _notOffsetOrCursorPageFailureStringResult;
        }

        var pageIsBothOffsetAndCursorPageResult = IsBothOffsetAndCursorPage();

        if (pageIsBothOffsetAndCursorPageResult.IsFailure)
        {
            return _bothOffsetAndCursorPageFailureStringResult;
        }

        if (Cursor is null)
        {
            return _offsetPageNameResult;
        }

        return _cursorPageResult;
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
