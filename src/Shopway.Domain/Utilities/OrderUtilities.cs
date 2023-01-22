using Shopway.Domain.Enums;

namespace Shopway.Domain.Utilities;

public static class OrderUtilities
{
    public static (SortDirection SortDirection, string Property) DetermineSortBy
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
            throw new InvalidOperationException("Multiple SortBy properties selected");
        }

        result.Property = result.Property[2..];

        return ((SortDirection, string))result!;
    }

    public static (SortDirection SortDirection, string Property) DetermineThenBy
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
            throw new InvalidOperationException("Multiple ThenBy properties selected");
        }

        result.Property = result.Property[6..];

        return ((SortDirection, string))result!;
    }
}