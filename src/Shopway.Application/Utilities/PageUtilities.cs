using Shopway.Application.Abstractions;
using Shopway.Application.Features;
using Shopway.Domain.Common.DataProcessing;

namespace Shopway.Application.Utilities;

public static class PageUtilities
{
    public static OffsetPageResponse<TResponse> ToPageResponse<TResponse>(this (IList<TResponse> Responses, int TotalCount) response, OffsetPage page)
        where TResponse : class, IResponse
    {
        return new OffsetPageResponse<TResponse>(response.Responses, response.TotalCount, page.PageSize, page.PageNumber);
    }

    public static CursorPageResponse<TResponse> ToPageResponse<TResponse>(this (IList<TResponse> Responses, Ulid NextCursor) response, CursorPage page)
        where TResponse : class, IResponse
    {
        return new CursorPageResponse<TResponse>(response.Responses, page.Cursor, response.NextCursor);
    }
}