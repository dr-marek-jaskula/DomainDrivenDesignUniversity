using Shopway.Domain.Enums;

namespace Shopway.Domain.Abstractions;

public interface ISortBy
{
    public (SortDirection Direction, string Property) SortBy { get; }
    public (SortDirection Direction, string Property) ThenBy { get; }

    protected static (SortDirection SortDirection, string Property) DetermineSortBy
    (
        params (SortDirection? SortDirection, string Property)[] items
    )
    {
        (SortDirection? SortDirection, string Property) result;

        try
        {
            result = items
                .Single(x => x.SortDirection is not null);
        }
        catch
        {
            throw new InvalidOperationException("Multiple SortBy properties selected.");
        }

        result.Property = result.Property[2..];

        return ((SortDirection, string))result!;
    }

    protected static (SortDirection SortDirection, string Property) DetermineThenBy
    (
        params (SortDirection? SortDirection, string Property)[] items
    )
    {
        (SortDirection? SortDirection, string Property) result;

        try
        {
            result = items
                .Single(x => x.SortDirection is not null);
        }
        catch
        {
            throw new InvalidOperationException("Multiple ThenBy properties selected.");
        }

        result.Property = result.Property[6..];

        return ((SortDirection, string))result!;
    }
}