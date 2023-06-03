# Persistence Layer :books:

In this layer we store components connected to the specific ORM and database operations:

- DatabaseContext
- UnitOfWork
- Configurations
- Specifications
- Repositories
- Migrations
- Outbox pattern classes
- Database Diagram

## Outbox pattern

This pattern is used to publish domain events. 

It is useful if we want to ensure that our transaction completes in a anatomic way.

Inside the transaction we generate one or more outbox messages and we save them in the outbox. 
Later, we process the outbox and publish the messages one by one, so they are handled by they respective consumers.

 Only AggregateRoots can rise domain events, so we need just:

 ```csharp
var outboxMessages = dbContext.ChangeTracker
	.Entries<AggregateRoot>()
```

The OutboxMessageConsumer is explained in ReadMe.Infrastructur.md chapter **Idempotency**

## Serialize the domain event

We should store the type of the serialized entity. This can be done by JsonSerializerSettings.TypeNameHandling set to All:

```csharp
JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings
{
    TypeNameHandling = typeNameHandling
});
```

This will help to deserialize object (when we are consuming the OutboxMessages) to IDomainEvent object that we can publish using mediator.

## Process the domain events in the background

We create a background job using the Quartz NuGet Package. This will be a part of a Shopway.Infrastructure layer.

## Specification Pattern

We represent the requirement that our database entities are supposed to meet, in order to satisfy the specification and to be returned from the database.

#### Filter & Order

Filtering and sorting is done by objects that implements IFilter<TEntity> and ISortBy<TEntity> interfaces. The apply method 
applied in the BaseRepository. To set the filtering in the specification we use 

```
specification
    .AddFilters(filter);

specification
    .AddOrder(sortBy);
```

We can also determine filtering and sorting in the specification without these objects, using 
```
specification
    .AddFilters(product => product.Id == productId);

specification
    .OrderBy(x => x.ProductName, SortDirection.Ascending)
    .ThenBy(x => x.Price, SortDirection.Descending);
```

## Fusion Cache

Fusion cache approach is used 

If no distributed cache is used, fusion cache use memory cache. Nevertheless, fusion cache is very efficient and easy to use.
If distributed cache is used, fusion cache use the combination of memory and distributed caching (prefer to use redis for distributed cache).

What is more the Backplane can be configured for multi node scenario. The preferred backplane is also redis.

A backplane is like a message bus (message broker) where change notifications will be published to all other connected nodes each time something happens to a cache entry, all automatically without us having to do anything.

```
# CORE PACKAGE
ZiggyCreatures.FusionCache

# SERIALIZER
ZiggyCreatures.FusionCache.Serialization.NewtonsoftJson

# DISTRIBUTED CACHE
Microsoft.Extensions.Caching.StackExchangeRedis

# BACKPLANE
ZiggyCreatures.FusionCache.Backplane.StackExchangeRedis
```

## Cache and Repository Pattern

In order not to involve any caching logic directly into repositories, we use decorator pattern and decorate repositories with cached repositories.
See "CachedProductRepository". 

Moreover, we use caching in "ReferenceValidationPipeline".

## Cache and UnitOfWork Pattern

In order to update values in the cache, the preferred way is to update them when SavingChanges.
We get from the change tracked all added and deleted values and update the cache.
Modified values are not required to be taken from change tracked, since here I use memory cache
and therefore the update is done automatically. 

In some specific scenarios we can update cache in the decorator.

We could add to SaveChanges a flag "updateCache" and add a new pipeline (plus add to CommandTransactionPipelineBase a method 
BeginTransactionWithoutCacheUpdateAsync) that uses this flag, if for some
endpoints the cache updates should be omitted.

## Indexes on ValueObjects with Owned Types

Microsoft currently aims to fully switch from Owned Types to Value Converter for ValueObject configurations:
```
It was previously the team view that owned entities, intended for aggregate support, would also be a reasonable approximation to value objects. 
Experience has shown this not to be the case. Therefore, in EF8, we plan to introduce a better experience focused on the needs of value objects in domain-driven design. 
This approach will be based on value converters rather than owned entities.
```

Therefore, this project uses ValueConverter and ValueCompares for ValueObject configurations.

The only "limitation" for this approach is that we need to add double casting in many expressions, otherwise we would not be able to use wrapped type possibilities.
Fore example in ProductFilter:

```csharp
return queryable
    .Filter(ByProductName, product => ((string)(object)product.ProductName).Contains(ProductName!))
    .Filter(ByRevision, product => ((string)(object)product.Revision).Contains(Revision!))
    .Filter(ByPrice, product => ((decimal)(object)product.Price) == Price)
    .Filter(ByUomCode, product => ((string)(object)product.UomCode).Contains(UomCode!));
```

We need to use "((string)(object)product.ProductName)" because otherwise we would not be able to call **Contains** method.

I hope that in Entity Framework Core 8 this will be handled less ugly.

## Dynamic or Static SortBy

Dynamic sort use Linq.Dynamic.Core library.

Static option for sorting the result:

```csharp
public sealed record ProductOrderStaticOption : ISortBy<Product>
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
            .SortBy(ProductName, product => product.ProductName.Value)
            .SortBy(Revision, product => product.Revision.Value)
            .SortBy(Price, product => product.Price.Value)
            .SortBy(UomCode, product => product.UomCode.Value);

        return ((IOrderedQueryable<Product>)queryable)
            .ThenSortBy(ThenProductName, product => product.ProductName.Value)
            .ThenSortBy(ThenRevision, product => product.Revision.Value)
            .ThenSortBy(ThenPrice, product => product.Price.Value)
            .ThenSortBy(ThenUomCode, product => product.UomCode.Value);
    }
}
```

Nevertheless, ISortBy and PageQueryValidator should be adjusted to static approach.