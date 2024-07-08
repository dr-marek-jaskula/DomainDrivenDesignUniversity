using Shopway.Domain.Common.Enums;
using System.Text.Json.Serialization;

namespace Shopway.Domain.Common.DataProcessing;

[JsonConverter(typeof(SortByEntryJsonConverter))]
public sealed record SortByEntry
{
    public required string PropertyName { get; init; }
    public SortDirection SortDirection { get; init; } = SortDirection.Ascending;
    public int SortPriority { get; set; }
    public bool ParsedFromString { get; init; } = false;

    public override string ToString()
    {
        return $"Property: {PropertyName}, SortDirection: {SortDirection}, SortPriority: {SortPriority}, ParsedFromString: {ParsedFromString}";
    }
}
