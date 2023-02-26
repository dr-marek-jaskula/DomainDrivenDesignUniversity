using Shopway.Application.Abstractions;

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

    public PageResponse(IList<TValue> items, int totalCount, int itemFrom, int itemsTo, int totalPages, int pageNumber)
    {
        Items = items;
        TotalItemsCount = totalCount;
        ItemsFrom = itemFrom;
        ItemsTo = itemsTo;
        TotalPages = totalPages;
        CurrentPage = pageNumber;
    }
}
