using Shopway.Domain.Common.Enums;
using Shopway.Domain.Common.Utilities;

namespace Shopway.Domain.Common.DataProcessing;

public sealed record SortByEntry
{
    public required string PropertyName { get; init; }
    public SortDirection SortDirection { get; init; } = SortDirection.Ascending;
    public int SortPriority { get; set; }
    public bool ParsedFromString { get; init; } = false;

    public static explicit operator SortByEntry(string value)
    {
        if (value.NotContains(":"))
        {
            return new SortByEntry()
            {
                PropertyName = value,
                ParsedFromString = true
            };
        }

        var splitted = value
            .Split(':')
            .Select(x => x.Trim());

        return new SortByEntry()
        {
            PropertyName = splitted.First(),
            SortDirection = splitted.Last() == "-1" 
                ? SortDirection.Descending 
                : SortDirection.Ascending,
            ParsedFromString = true
        };
    }

    public override string ToString()
    {
        return $"Property: {PropertyName}, SortDirection: {SortDirection}, SortPriority: {SortPriority}, ParsedFromString: {ParsedFromString}";
    }
}