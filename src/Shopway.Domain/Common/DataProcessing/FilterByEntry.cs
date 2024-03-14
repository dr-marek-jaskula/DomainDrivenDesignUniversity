namespace Shopway.Domain.Common.DataProcessing;

/// <summary>
/// List of FilterEntries determines expression that is logic AND of all filterEntries
/// </summary>
public sealed record FilterByEntry
{
    public FilterByEntry(IList<Predicate> predicates, string propertyName, string operation, object Value)
    {
        if (predicates is not null)
        {
            Predicates = predicates;
            return;
        }

        Predicates = [ new Predicate
        {
            PropertyName = propertyName,
            Operation = operation,
            Value = Value
        }];
    }

    /// <summary>
    /// List of predicates determines expression that is logic OR of all predicates
    /// </summary>
    public IList<Predicate> Predicates { get; init; } = [];


    /// <summary>
    /// Each predicate determines one expression
    /// </summary>
    public sealed record Predicate
    {
        public required string PropertyName { get; init; }
        public required string Operation { get; init; }
        public required object Value { get; init; }

        public override string ToString()
        {
            return $"Property: {PropertyName}, Operation: {Operation}, Value: {Value}";
        }
    }
}