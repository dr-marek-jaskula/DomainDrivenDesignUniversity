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

    public PageResponse(IList<TValue> items, int totalCount, int pageSize, int pageNumber)
    {
        Items = items;
        TotalItemsCount = totalCount;
        ItemsFrom = pageSize * (pageNumber - 1) + 1;
        ItemsTo = Math.Min(ItemsFrom + pageSize - 1, totalCount);
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        CurrentPage = pageNumber;
    }
}
