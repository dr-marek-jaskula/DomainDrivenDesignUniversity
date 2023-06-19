namespace Shopway.Domain.Common;

/// <summary>
/// List of FilterEntries determines expression that is logic AND of all filterEntries
/// </summary>
public sealed record FilterByEntry
{
    /// <summary>
    /// List of predicates determines expression that is logic OR of all predicates
    /// </summary>
    public required IList<Predicate> Predicates { get; init; }

    /// <summary>
    /// Each predicate determines one expression
    /// </summary>
    public sealed record Predicate
    {
        public required string PropertyName { get; init; }
        public required string Operation { get; init; }
        public required object Value { get; init; }
    }
}