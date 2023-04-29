using Shopway.Domain.Common;
using Shopway.Application.CQRS;
using Shopway.Application.Abstractions;

namespace Shopway.Application.Utilities;

public static class PageUtilities
{
    public static PageResponse<TResponse> ToPageResponse<TResponse>(this (IList<TResponse> Responses, int TotalCount) response, Page page)
        where TResponse : class, IResponse
    {
        return new PageResponse<TResponse>(response.Responses, response.TotalCount, page.PageSize, page.PageNumber);
    }
}