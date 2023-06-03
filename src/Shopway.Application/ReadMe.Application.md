# Application Layer :computer:

In this layer we define: 

- CQRS abstraction and implementation
- CQRS validation
- pagination
- responses (sometimes called "DTOs" or "Dtos")
- mappings (do not use AutoMapper, use custom made mappers instead!)
- request pipelines

## Validators

1. FleuntValidator. Mostly deal with the combinations of the request parameters. 

Example:
```csharp
RuleFor(m => new {m.CountyId, m.Zip}).Must(x => ValidZipCounty(x.Zip, x.CountyId))
```

Or see **ProductPageQueryValidator**.

2. ReferenceValidation. MediatR pipeline that checks if the given id truly refers to the entity.
	Therefore, verifying in the handler, that the entity is not null, is redundant.

3. The remaining validation is done in the domain layer.

4. Use Validator custom tool, defined in .Infrastructure layer. It is very helpful and simple tool, thus I encourage to get familiar with it.

## Transaction Pipelines

There is not need to save changes inside handlers, because it is done in the transaction pipeline.
Moreover, the pipeline provides the transaction scope, so if at least one database operation fail, all operations are rollbacked.
Therefore in some cases, we return a task, which slightly increases the performance.

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
    .PageAsync(pageQuery.Page, pageQuery.Filter, pageQuery.Order, ProductMapping.ProductResponse, cancellationToken);
```

3. Use the expression as a parameter of the **Select** method. The repository method must be a generic one:

```csharp
public async Task<(IList<TResponse> Responses, int TotalCount)> PageAsync<TResponse>
(
    IPage page, 
    IFilter<Product>? filter, 
    ISortBy<Product>? sort, 
    Expression<Func<Product, TResponse>>? select, 
    CancellationToken cancellationToken, 
    params Expression<Func<Product, object>>[] includes
)
{
    var specification = ProductQuerySpecification<TResponse>.Create(filter, sort, select, includes);
        
    return await UseSpecificationWithMapping(specification)
        .PageAsync(page, cancellationToken);
}
```

Note: in this solution we use the **SpecificationPattern**. Therefore, the expression is applied using the concrete specification.

```charp
private protected IQueryable<TResponse> UseSpecificationWithMapping<TEntity, TEntityId, TResponse>(SpecificationWithMappingBase<TEntity, TEntityId, TResponse> specification)
    where TEntityId : IEntityId
    where TEntity : Entity<TEntityId>
{
    if (specification.Select is null)
    {
        throw new ArgumentNullException($"SpecificationWithMappingBase must contain Select statement");
    }

    return UseSpecificationWithoutMapping(specification)
        .Select(specification.Select);
}
```

**In the end the queryable will contain the mapping, so no redundant data will be queried, and the .Persistence layer will remain separated from .Application layer.**