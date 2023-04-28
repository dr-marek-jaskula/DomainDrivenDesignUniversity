using Shopway.Application.Abstractions;
using Shopway.Application.CQRS;

namespace Shopway.Application.Utilities;

public static class PageUtilities
{
    public static PageResponse<TResponse> ToPageResponse<TResponse>(this (IList<TResponse> Responses, int Count) response, Page page)
        where TResponse : class, IResponse
    {
        return new PageResponse<TResponse>(response.Responses, response.Count, page.PageSize, page.PageNumber);
    }
}