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

## Proxy Service and Fully Dynamic Endpoint

I introduced a ```MediatorProxyService``` that is responsible for converting the proxy request to the concrete query that will be used by sander to find the respective handler.
This solution is fully open-closed, thus the only think we would need to add new functionality is just to add a new method and decorate it with ```QueryStrategy``` attribute.
This could be done in on class, but for the sake of readability, it is better to split it into multiple files, but still keep in one class (the use of partial class is 
justified). 

Endpoint ```/proxy/query``` (see **ProxyController**) uses ```MediatorProxySerive```, so there is a single endpoint that can query paginated data in multiple ways:
- we can use any supported entity
- we can use ```OffsetPage``` or ```CursorPage```
- we can use any dynamic filtering (also LIKE statement is supported)
- we can use any dynamic sorting
- we can use any dynamic mapping

Flexibility of this solution was the main idea behind its implementation. Nevertheless, the performance is also impressive despite some boxing that can occur (DataTansferObject value type is ```object```).

### Ultimate Generic Proxy Endpoints

Proxy endpoints (for instance ones defined by FastEndpoints) with `generic` in path are capable to use only one generic handler and one generic repository to query:
- any registered entity by id
- any registered entity by key that implements generic IUniqueKey interface
- offset page of any registered entities
- cursor page of any registered entities

Therefore, it is the ultimate solution - supporting new entities requires only adding a new strategy methods in MediatorProxyService like:

```csharp
//Users
[GenericPageQueryStrategy(nameof(User), typeof(OffsetPage))]
private static GenericOffsetPageQuery<User, UserId> GenericQueryUsersUsingOffsetPage(GenericProxyPageQuery proxyQuery)
    => GenericOffsetPageQuery<User, UserId>.From(proxyQuery);

[GenericPageQueryStrategy(nameof(User), typeof(CursorPage))]
private static GenericCursorPageQuery<User, UserId> GenericQueryUsersUsingCursorPage(GenericProxyPageQuery proxyQuery)
    => GenericCursorPageQuery<User, UserId>.From(proxyQuery);
```

and ones for id

```csharp
//Users
[GenericByIdQueryStrategy(nameof(User))]
private static GenericByIdQuery<User, UserId> GenericQueryUserById(GenericProxyByIdQuery proxyQuery)
    => GenericByIdQuery<User, UserId>.From(proxyQuery);
```

for supporting key for User entity, we would need to create an UserKey like this: 

```csharp
public readonly record struct UserKey : IUniqueKey<User, UserKey>
{
    ...
}
```

I assume that we can uniquely query one element only by Id or by UniqueKey. If we need more unique values that we can query by, just implement similar solution  for UniqueKey approach.

Its is preferred to use FastEndpoint for these ultimate strategy to obtain the best possible performance.

It is considerable to use caching for these queries, but for the tutorial purpose it is not used here. To add caching simply implement ICachedQuery interface or go decorator approach - use IFusionCache or upcoming `HybridCache`, however IFusionCache seems to be a easy winner.
