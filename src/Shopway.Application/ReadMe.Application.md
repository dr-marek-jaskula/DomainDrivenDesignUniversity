# Application Layer :computer:

In this layer we define: 

- CQRS abstraction and implementation
- CQRS validation
- pagination
- responses (sometimes called "DTOs" or "Dtos")
- mappings (do not use AutoMapper, use custom made mappers instead!)
- middlewares and pipelines (that are not EF Core specific)

## Validators

1. FleuntValidator. Mostly deal with the combinations of the request parameters. 

Example:
```csharp
RuleFor(m => new {m.CountyId, m.Zip}).Must(x => ValidZipCounty(x.Zip, x.CountyId))
```

Or see **PageQueryValidator** or other page validators.

2. The remaining validation is done in the domain layer.

3. Use Validator custom tool, defined in .Infrastructure layer. It is very helpful and simple tool, thus I encourage to get familiar with it.

## Paginations

There are two types of paginations:

1. Offset pagination (see **ProductDictionaryOffsetPageQueryHandler**)
2. Cursor pagination (see **ProductDictionaryCursorPageQueryHandler**)

Both of them have their advantages and disadvantages.

Standard offset pagination can give us informations about items total count and total amout of pages. This technique is easy to implement and easy to maintain.
In this methodology, we specify the **PageSize** and **PageNumber**. As a response, we obtain items with additional informations. We can also use approach of providing the **Limit** and **Offset**, if the team prefers that way.

Cursor pagination is a way more efficient methodology when we struggle with massive databases. Nevertheless in this approach, we do not get the information about the total count. Thus, it is good for infinite scrolling.
We specify the **PageSize** and the **Cursor** that is the **Ulid** (id) of the record from which we want to start from. 
As a response, we get items and the next cursor which is the **Ulid** (id) of the next record. If we reached the last record, then we return the ```Ulid.Empty``` or Ulid of the last record (team should decide).
For this project I use the ```Ulid.Empty``` as a next cursor if the last element was reached. Moreover, if we provide the ```Ulid.Empty``` as a cursor, we will retrieve all records. 
If such behavior is undesirable, then we can validate the input to deny the ```Ulid.Empty``` as an input parameter (in ```CursorPageQueryValidator```). 

Note: for cursor pagination the response must implement the **IHasCursor** interface.

## Mapping 

Manual mapping is way faster than mapping done by external libraries like automapper. Therefore, manual mapping
is done in "Mapping" folder.

**Moreover, manual mapping plays a significant role when using the repository pattern.**

Problem statement: 
1. Persistence layer should not have access to response dtos (like **ProductResponse**)
2. We do not want to query redundant data.
3. Therefore, repository method should be aware of the required mapping, but at the same time the data transfer model should be out of Persistence layer scope.

Solution:
1. Create mapping that returns an expression of func in mapping folder (.Application layer):

```csharp
public static Expression<Func<Product, ProductResponse>> ProductResponse => product => new ProductResponse
(
    product.Id.Value,
    product.ProductName.Value,
    product.Revision.Value,
    product.Price.Value,
    product.UomCode.Value,
    product.Reviews
        .Select(review => new ReviewResponse
        (
            review.Id.Value,
            review.Username.Value,
            review.Stars.Value,
            review.Title.Value,
            review.Description.Value
        ))
        .ToList()
        .AsReadOnly()
);
```

2. Pass the expression as a parameter of the repository method:

```csharp
var page = await _productRepository
    .PageAsync(pageQuery.Page, cancellationToken, staticFilter: pageQuery.Filter, staticSort: pageQuery.SortBy, mapping: ProductMapping.ProductResponse);
```

3. Use the expression as a parameter of the **Select** method. The repository method must be a generic one:

```csharp
public async Task<(IList<TResponse> Responses, int TotalCount)> PageAsync<TResponse>
(
    IPage page,
    CancellationToken cancellationToken,
    IDynamicFilter<Product>? dynamicFilter = null,
    IStaticFilter<Product>? staticFilter = null,
    IStaticSortBy<Product>? staticSort = null,
    IDynamicSortBy<Product>? dynamicSort = null,
    Expression<Func<Product, TResponse>>? mapping = null,
    params Expression<Func<Product, object>>[] includes
)
{
    var specification = CommonSpecification.WithMapping<Product, ProductId, TResponse>.Create
    (
        staticFilter,
        dynamicFilter,
        staticSort,
        dynamicSort, 
        mapping: mapping, 
        includes: includes
    );

    return await UseSpecification(specification)
        .PageAsync(page, cancellationToken);
}
```

Note: in this solution we use the **SpecificationPattern**. Therefore, the expression is applied using the concrete specification.

```charp
private protected IQueryable<TResponse> UseSpecification<TEntity, TEntityId, TResponse>(SpecificationWithMappingBase<TEntity, TEntityId, TResponse> specification)
    where TEntityId : struct, IEntityId
    where TEntity : Entity<TEntityId>
{
    if (specification.Mapping is null)
    {
        throw new ArgumentNullException($"SpecificationWithMappingBase must contain Select statement");
    }

    return UseSpecification((SpecificationBase<TEntity, TEntityId>)specification)
        .Select(specification.Mapping);
}
```

**In the end the queryable will contain the mapping, so no redundant data will be queried, and the .Persistence layer will remain separated from .Application layer.**

## Logging using SourceGenerated code

The most efficient way is to use SourceGeneration logging. It can be obtained by using the **LoggerMessageAttribute**.
See examples in **LoggingPipeline**. To further increase performance of logs we can manage **SkipEnabledCheck** option. If, we 
are sure that the log level will not exceed the one in the current log, then we can set SkipEnabledCheck to **true**. Otherwise, its better to keep it as **false**.

Logging is a crucial feature for almost any application. Nevertheless, most of developers do not pay enough attention on the massive performance impact that logs can have.
Gaining even small boosts in this field can significantly improve the overall application performance. Thus, in this project we aim to reach the most efficient way of logging.


## Filter & Order

Filtering and sorting is done by objects that implements ```IFilter<TEntity>``` and ```ISortBy<TEntity>``` interfaces. The apply method 
invoked in UseSpecification method that is located in the BaseRepository. The dynamic filtering and sorting can be obtained by objects 
that implements ```IDynamicFilter<TEntity>``` and ```IDynamicSortBy<TEntity>```. To set the filtering/sorting in the specification we use 

```csharp
specification
    .AddFilter(filter);

specification
    .AddSortBy(sortBy);
```

We can also determine filtering and sorting in the specification without these objects, using 
```
specification
    .AddFilters(product => product.Id == productId);

specification
    .OrderBy(x => x.ProductName, SortDirection.Ascending)
    .ThenBy(x => x.Price, SortDirection.Descending);
```

## Dynamic or Static Filter

Dynamic filter creates expression trees. Method that handles creating and applying them is the extension method in **QueryableUtilities**
in the Domain called **Where**.

Principles:
1. Each FilterByEntry will generate one or more expressions (see point 3.)
   
   Example: ```Price < 15```

2. Final expression is a combination of all FilterByEntry expressions separated by AND operator 

    Example: ```Price < 15 && Name == "Marek"```

3. Each FilterByEntry generates multiple expressions that are separated by OR operator

    Example: ```Price < 15 || Description = "Fine"```

4. Each expression from multiple FilterByEntry expression is generated based on a single Predicate
5. Each FilterByEntry contains a list of Predicates 

Final expression will look like this:

```(Price < 15 || Description = "Fine") && Name == "Marek"```

To sum up, each expression created from Predicate will be separated by ||, and each predicate created from FilterByEntry
will be separated by &&.

Therefore, we dynamically create any expression we need. 

**Moreover**, we can extend each dynamic filter in the **Apply** method by any static filters, so we have full control on 
filtering.

Static option for filtering the result applies the predicates we have written on our own:

```csharp
public sealed record ProductStaticFilter : IFilter<Product>
{
    public string? ProductName { get; init; }
    public string? Revision { get; init; }
    public decimal? Price { get; init; }
    public string? UomCode { get; init; }

    private bool ByProductName => ProductName.NotNullOrEmptyOrWhiteSpace();
    private bool ByRevision => Revision.NotNullOrEmptyOrWhiteSpace();
    private bool ByPrice => Price.HasValue;
    private bool ByUomCode => UomCode.NotNullOrEmptyOrWhiteSpace();

    public IQueryable<Product> Apply(IQueryable<Product> queryable)
    {
        return queryable
            .Filter(ByProductName, product => ((string)(object)product.ProductName).Contains(ProductName!))
            .Filter(ByRevision, product => ((string)(object)product.Revision).Contains(Revision!))
            .Filter(ByPrice, product => ((decimal)(object)product.Price) == Price)
            .Filter(ByUomCode, product => ((string)(object)product.UomCode).Contains(UomCode!));
    }
}
```

## Dynamic or Static SortBy

Dynamic sort use Linq.Dynamic.Core library.

Static option for sorting the result:

```csharp
public sealed record ProductStaticSortBy : ISortBy<Product>
{
    public SortDirection? ProductName { get; init; }
    public SortDirection? Revision { get; init; }
    public SortDirection? Price { get; init; }
    public SortDirection? UomCode { get; init; }

    public SortDirection? ThenProductName { get; init; }
    public SortDirection? ThenRevision { get; init; }
    public SortDirection? ThenPrice { get; init; }
    public SortDirection? ThenUomCode { get; init; }

    public IQueryable<Product> Apply(IQueryable<Product> queryable)
    {
        queryable = queryable
            .SortBy(ProductName, product => product.ProductName)
            .SortBy(Revision, product => product.Revision)
            .SortBy(Price, product => product.Price)
            .SortBy(UomCode, product => product.UomCode);

        return ((IOrderedQueryable<Product>)queryable)
            .ThenSortBy(ThenProductName, product => product.ProductName)
            .ThenSortBy(ThenRevision, product => product.Revision)
            .ThenSortBy(ThenPrice, product => product.Price)
            .ThenSortBy(ThenUomCode, product => product.UomCode);
    }
}
```

## Customize Dynamic or Static Filtering/Sorting

We can add any custom conditions (filters/sort statements) into "Apply" method for additional filtering/sorting.

We can also combine dynamic and static filtering/sorting.

Moreover, we can combine dynamic/static filtering/sorting with manual approach.

Example of manual approach:
```
specification
    .AddFilters(product => product.Id == productId);
```