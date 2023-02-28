using Microsoft.AspNetCore.Mvc.Diagnostics;
using Shopway.Application.Abstractions;
using Shopway.Application.Exceptions;

namespace Shopway.Application.CQRS;

public sealed class PageResponse<TValue> : IResponse
{
    //Generic list that stores the pagination result
    public IList<TValue> Items { get; set; }

    //Total number or items
    public int TotalItemsCount { get; set; }

    //Total amount of pages
    public int TotalPages { get; set; }

    public int CurrentPage { get; set; }

    //The first element of the certain page
    public int ItemsFrom { get; set; }

    //The last element of the certain page
    public int ItemsTo { get; set; }

    public PageResponse(IList<TValue> items, int totalCount, int pageSize, int pageNumber)
    {
        Items = items;
        CurrentPage = pageNumber;
        TotalItemsCount = totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        if (CurrentPage > TotalPages)
        {
            throw new BadRequestException($"Selected page '{CurrentPage}' is greater then total number of pages '{TotalPages}'");
        }

        ItemsFrom = Math.Min(pageSize * (pageNumber - 1) + 1, totalCount);
        ItemsTo = Math.Min(ItemsFrom + pageSize - 1, totalCount);
    }
}
