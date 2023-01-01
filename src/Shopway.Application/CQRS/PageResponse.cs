namespace Shopway.Application.CQRS;

public sealed class PageResponse<TValue>
{
    //Generic list that stores the pagination result
    public List<TValue> Items { get; set; }

    //Total number or items
    public int TotalItemsCount { get; set; }

    //Total amount of pages
    public int TotalPages { get; set; }

    //The first element of the certain page
    public int ItemsFrom { get; set; }

    //The last element of the certain page
    public int ItemsTo { get; set; }

    public PageResponse(List<TValue> items, int totalCount, int pageSize, int pageNumber)
    {
        Items = items;
        TotalItemsCount = totalCount;

        //FirstElement
        ItemsFrom = pageSize * (pageNumber - 1) + 1;

        //LastElement
        ItemsTo = ItemsFrom + pageSize - 1;

        //Total number of pages
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }
}
