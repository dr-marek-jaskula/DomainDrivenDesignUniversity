using Shopway.Domain.Enums;

namespace Shopway.Application.Abstractions;

public interface ISortBy
{
    public (SortDirection Direction, string Property) SortBy { get; }
    public (SortDirection Direction, string Property) ThenBy { get; }

    protected static (SortDirection SortDirection, string Property) DetermineSortBy
    (
        params (SortDirection? SortDirection, string Property)[] items
    )
    {
        var result = items
            .Single(x => x.SortDirection is not null);

        result.Property = result.Property[2..];

        return ((SortDirection, string))result!;
    }

    protected static (SortDirection SortDirection, string Property) DetermineThenBy
    (
        params (SortDirection? SortDirection, string Property)[] items
    )
    {
        var result = items
            .Single(x => x.SortDirection is not null);

        result.Property = result.Property[6..];

        return ((SortDirection, string))result!;
    }
}