using Shopway.Application.Exceptions;
using Shopway.Application.Abstractions;

namespace Shopway.Application.Features;

public sealed record OffsetPageResponse<TValue> : IResponse
{
    /// <summary>
    /// Generic list that stores the pagination result
    /// </summary>
    public IReadOnlyList<TValue> Items { get; private init; }

    /// <summary>
    /// Total number or items
    /// </summary>
    public int TotalItemsCount { get; private init; }

    /// <summary>
    /// Total amount of pages
    /// </summary>
    public int TotalPages { get; private init; }

    /// <summary>
    /// Selected page
    /// </summary>
    public int CurrentPage { get; private init; }

    /// <summary>
    /// The first element of the certain page
    /// </summary>
    public int ItemsFrom { get; private init; }

    /// <summary>
    /// The last element of the certain page
    /// </summary>
    public int ItemsTo { get; private init; }

    /// <summary>
    /// True if current page is not the first page
    /// </summary>
    public bool HasPreviousPage => CurrentPage > 1;

    /// <summary>
    /// True if current page is not the last page
    /// </summary>
    public bool HasNextPage => CurrentPage < TotalPages;

    public OffsetPageResponse(IList<TValue> items, int totalCount, int pageSize, int pageNumber)
    {
        Items = items.AsReadOnly();
        CurrentPage = pageNumber;
        TotalItemsCount = totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        if (CurrentPage > TotalPages && TotalItemsCount > 0)
        {
            throw new BadRequestException($"Selected page '{CurrentPage}' is greater than total number of pages '{TotalPages}'");
        }

        ItemsFrom = Math.Min(pageSize * (pageNumber - 1) + 1, totalCount);
        ItemsTo = Math.Min(ItemsFrom + pageSize - 1, totalCount);
    }
}
