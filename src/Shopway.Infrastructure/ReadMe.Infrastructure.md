# Infrastructure Layer

This layer is responsible to implement the Contracts (Interfaces/Adapters) defined within the application layer to the Secondary Actors. Infrastructure Layer supports other layer by implementing the abstractions and integrations to 3rd-party library and systems.

Infrastructure layer contains most of your application’s dependencies on external resources such as file systems, web services, third party APIs, and so on. The implementation of services should be based on interfaces defined within the application layer.

In this project we store components connected to :

- Background jobs
- Providers
- Adapters
- Decorators
- Validators
- Services

## Bacground jobs by Quartz

Background jobs from Quartz NuGet Package have Scoped lifetime.
Therefore, we can inject scoped services.

In Shopway.App to configure Quartz background jobs we need Quartz.Extensions.Hosting NuGet Package. This integrates with background service in ASP.NET.

## Idempotency

This means: being able to execute a certain operation multiple times without changing an initial result.

In other words it is like: on retry perform only these operation that were not processed.

Example: we are publishing the domain event that has 3 event handlers. Lets assume that event handler 1 and 2 execute properly, but event handler 3 fails.
This effects in publishing the domain event again, because of the retry mechanism. Because of this, the event handler 1 and 2 will be called again.
If one of this event handler would deal with payment, the customer would be charged twice.

To deal with this problem we define **OutboxMessageConsumer** class in the Persistence Layer. 
It contains the id that matches the id of OutboxMessage. It also contains the name, that will come from the event handler that is being executed.
Therefore, we configure this entity to contain the compose key:

```csharp
builder.HasKey(outboxMessageConsumer => new
{
    outboxMessageConsumer.Id,
    outboxMessageConsumer.Name
});
```

Every time we publish the domain event, before handling the actual event, we check in OutboxMessageConsumer if we already processed this message for this event handler.
If so we want to skip it, otherwise we execute the event handler, and we add a new record to the OutboxMessageConsumer table.

In fact, this will be obtain by the use of the Decorator Pattern. We create **IdempotentDomainEventHandlerDecorator** that decorates INotificationHandler<IDomainEvent>.

- At first we get the name of the type of the decorated handler (this will be OutboxMessageConsumer.Name)
- Examine if the decorated handler was already processed
	- If so, skip this handler. Otherwise, proceed (handle the domain event).
- After the handler is processed, add to database a new OutboxMessageConsumer with domain event id and handler name.
	- SaveChangesAsync