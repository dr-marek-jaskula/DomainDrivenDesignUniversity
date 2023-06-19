namespace Shopway.Domain.Common;

//TODO: 
//1. Make this to have a property that inherits from IEnumerable (called mb WhereExpression?)
//2. Add behavior that combines the expression from this data
//3. Each item of this WhereExpression should be joined using OR operator
//4. Each FilterByEntry should be join then by AND operator (this behavior can be added here also)
//5. Extension for IQueryable should just use this behavior
//6. Adjust postman and push new collection (adjust ReadMe.md)

//Extract some logic for this extension method to SoryByEntry

public sealed record FilterByEntry
{
    //public required IList<Predicate> Predicates { get; init; }
    public required string PropertyName { get; init; }
    public required string Operation { get; init; }
    public required object Value { get; init; }
}

//public sealed record Predicate
//{
//    public required string PropertyName { get; init; }
//    public required string Operation { get; init; }
//    public required object Value { get; init; }
//}