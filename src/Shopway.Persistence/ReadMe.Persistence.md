# Persistence Layer :books:

In this layer we store components connected to the specific ORM, database operations, and database related implementations:

- Background jobs
- DatabaseContext
- UnitOfWork
- Configurations
- Specifications
- Repositories
- Migrations
- Outbox pattern
- EF Core specific pipelines
- MediatR

## Background jobs by Quartz

Background jobs from Quartz NuGet Package have Scoped lifetime.
Therefore, we can inject scoped services.

## Delete OutDated SoftDeleted Entities

Background Job **DeleteOutdatedSoftDeletableEntitiesJob** runs once per month and gets at runtime 
all entities that implements ISoftDeletable interface and then permanently deletes entities that 
were soft deleted one year in the past.

## Redis Cache

1. Preferred way: see ReadMe.Persistence for **FusionCache**
2. Create CacheOptions
3. Use caching, for instance in the decorator (see CachedProductRepository)
4. Add PrivateResolver (in Shopway.Persistence) to be able to deserialized object with private setters
5. Update cache in UniteOfWork (see Persistence layer) or in the decorator 

IMemory cache is faster, but the Redis cache can be reused between multiple instance of the same application.

If we do not want to use decorator pattern for cache, we can create a CacheService or a CachePipeline

NOTE: When dealing with aggregates, it is common that to remove a entity from the aggregate root, we also want to clear the cache.
The approach with using the UpdateCache in UnitOfWork like this:
```csharp
    private void UpdateCache()
    {
        IEnumerable<EntityEntry<IAggregateRoot>> entries =
            _dbContext
                .ChangeTracker
                .Entries<IAggregateRoot>()
                .Where(entity => entity.State is Deleted);

        foreach (var entityEntry in entries)
        {
            var entityId = entityEntry.Entity.GetEntityIdFromEntity();

            if (entityEntry.State is Deleted)
            {
                _fusionCache.Remove(entityId);
            }
        }
    }
```

will not resolve this particular issue, because when the entity is removed from the aggregate, for instance in this manner:
```csharp
public bool RemoveReview(Review review)
{
    return _reviews.Remove(review);
}
```

and the ChangeTracker will not set Review entry state to **Deleted**, so the cache will not be cleared. 

To handle this problem we can either clean cache directly in the application handler or use domain events, which is more resilient solution.
Therefore, we need to add
```csharp
public bool RemoveReview(Review review)
{
    RaiseDomainEvent(ReviewRemovedDomainEvent.New(review.Id, Id));
    return _reviews.Remove(review);
}
```

and the DomainEventHandler
```csharp
internal sealed class ClearCacheWhenReviewRemovedDomainEventHandler(IFusionCache fusionCache) : IDomainEventHandler<ReviewRemovedDomainEvent>
{
    private readonly IFusionCache _fusionCache = fusionCache;

    public Task Handle(ReviewRemovedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        _fusionCache.Remove(domainEvent.ReviewId);
        return Task.CompletedTask;
    }
}
```

Then, the cache will be cleared by the background job, just after some delay. Nevertheless, approach with UnitOfWork will solve all cases which 
for aggregate roots.

NOTE: When using a MemoryCache, we do not need to remove from the cache the entities which were modified, because the modification will
be set if happens. 

NOTE: Even if the SaveChanges fail and the transaction is rollback, we potentially only removed entity from the cache, which will not have any impact other
than one more round trip to database.

## Spin the Docker Container with Redis

Create and run the docker container from the redis images in the detached mode

```cmd
docker run -p 6379:6379 --name redis -d redis
```

## Reference pipeline 

MediatR pipeline that checks if the given id truly refers to the entity.
Therefore, verifying in the handler, that the entity is not null, is redundant.

## Transaction Pipelines

There is no need to save changes inside handlers, because it is done in the transaction pipeline.
Moreover, the pipeline provides the transaction scope, so if at least one database operation fail, all operations are rollbacked.
Therefore in some cases, we return a task, which slightly increases the performance.

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

## Fusion Cache

Fusion cache approach is used.

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

## ValueObjects vs OwnedTypes

Microsoft currently aims to fully switch from Owned Types to Value Converter for ValueObject configurations:
```
It was previously the team view that owned entities, intended for aggregate support, would also be a reasonable approximation to value objects. 
Experience has shown this not to be the case. Therefore, in EF8, we plan to introduce a better experience focused on the needs of value objects in domain-driven design. 
This approach will be based on value converters rather than owned entities.
```

Therefore, this project uses ValueConverter and ValueCompares for ValueObject configurations.

The only "limitation" for this approach is that we need to add double casting in many expressions, otherwise we would not be able to use wrapped type possibilities.

For example in ProductStaticFilter:

```csharp
return queryable
    .Filter(ByProductName, product => ((string)(object)product.ProductName).Contains(ProductName!))
    .Filter(ByRevision, product => ((string)(object)product.Revision).Contains(Revision!))
    .Filter(ByPrice, product => ((decimal)(object)product.Price) == Price)
    .Filter(ByUomCode, product => ((string)(object)product.UomCode).Contains(UomCode!));
```

We need to use "((string)(object)product.ProductName)" because otherwise we would not be able to call **Contains** method.

## Open Telemetry

All configuration is done in **OpenTelemetryRegistration.cs** file.

If project is run directly, then singlas will be exported to the console. Otherwise, when we use **docker-compose**, we will export singlas to **Open Telemetry Collector**.

Then, signals are exported to Jaeger (see http://localhost:16686) and Prometheus (see http://localhost:9090) (plus Grafana at http://localhost:3000). 
Additionally, all signals are exported to Aspire Dashboard (see http://localhost:18888).

In order to correlate traces with logs (logs configuration) we use **ActivityEventLogProcessor**:
```csharp
private class ActivityEventLogProcessor : BaseProcessor<LogRecord>
{
    public override void OnEnd(LogRecord log)
    {
        base.OnEnd(log);
        Activity.Current?.AddEvent(new ActivityEvent(log.FormattedMessage!));
    }
}
```

There is also OpenTelemetryBase class in **.Application** layer that is used as a base class for custom metrics. Example custom metric is **CreateOrderHeaderOpenTelemetry**
that is located directly in the CreateOrderHeader feature, both with **CreateOrderHeaderOpenTelemetryPipeline** to utilize these metrics.

#### Obsolete **Serilog** configuration:

Program.cs
```csharp
//Log.Logger = CreateSerilogLogger();

try
{
    Log.Information("Staring the web host");

    //Initial configuration

    var builder = WebApplication.CreateBuilder(new WebApplicationOptions
    {
        Args = args,
        ContentRootPath = Directory.GetCurrentDirectory()
    });

    builder.ConfigureSerilog();
...
}
catch (Exception exception)
{
    Log.Fatal(exception, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.Information("Ending the web host");
    Log.CloseAndFlush();
}
```

Logger Utilities:
```csharp
public static class LoggerUtilities
{
    private const string Microsoft = nameof(Microsoft);

    public static Serilog.ILogger CreateSerilogLogger()
    {
        return new LoggerConfiguration()
            .MinimumLevel.Override(Microsoft, Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();
    }

    public static void ConfigureSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext());
    }
}
```

appsettings.json 
```json
"Serilog": {
  "Using": [ "Serilog.Sinks.Console", "Serilog.Sinks.File" ],
  "MinimumLevel": {
    "Default": "Information",
    "Override": {
      "Microsoft": "Warning",
      "System": "Warning"
    }
  },
  "WriteTo": [
    {
      "Name": "Console",
      "Args": {
        "restrictedToMinimumLevel": "Warning"
      }
    },
    {
      "Name": "File",
      "Args": {
        "path": "Logs/log-.json", //this '-' determines that the current timestamp will be appended to the log file name
        "rollingInterval": "Day",
        "formatter": "Serilog.Formatting.Compact.CompactJsonFormatter, Serilog.Formatting.Compact"
      }
    }
  ],
  "Enrich": [ "FromLogContext", "WithMachineName", "WithProcessId", "WithThreadId" ]
}
```