# Persistence Layer

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
Content = JsonConvert.SerializeObject(
    domainEvent,
    new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.All
    })
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
    .OrderBy(x => x.ProductName.Value, SortDirection.Ascending)
    .ThenBy(x => x.Price.Value, SortDirection.Descending);
```

## Cache

Basic memory cache is presented in the "CachedOrderRepository"

Redis cache is presented in the "CachedProductRepository"