using Shopway.Application.Abstractions;
using Shopway.Application.Exceptions;

namespace Shopway.Application.CQRS;

public sealed record PageResponse<TValue> : IResponse
{
    //Generic list that stores the pagination result
    public IList<TValue> Items { get; private init; }

    //Total number or items
    public int TotalItemsCount { get; private init; }

    //Total amount of pages
    public int TotalPages { get; private init; }

    //Selected page
    public int CurrentPage { get; private init; }

    //The first element of the certain page
    public int ItemsFrom { get; private init; }

    //The last element of the certain page
    public int ItemsTo { get; private init; }

    public PageResponse(IList<TValue> items, int totalCount, int pageSize, int pageNumber)
    {
        Items = items;
        CurrentPage = pageNumber;
        TotalItemsCount = totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        if (CurrentPage > TotalPages && TotalItemsCount > 0)
        {
            throw new BadRequestException($"Selected page '{CurrentPage}' is greater then total number of pages '{TotalPages}'");
        }

        ItemsFrom = Math.Min(pageSize * (pageNumber - 1) + 1, totalCount);
        ItemsTo = Math.Min(ItemsFrom + pageSize - 1, totalCount);
    }
}
