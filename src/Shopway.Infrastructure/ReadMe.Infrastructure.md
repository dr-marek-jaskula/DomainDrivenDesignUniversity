# Infrastructure Layer

This layer is responsible for providing the implementations for abstractions for other layers, mostly for .Application layer.
Moreover, this layer contains most tools and additional services.

Therefore, in this project we store:

- Background jobs
- Providers
- Adapters
- Decorators
- Validators
- Services
- Authorization components
- Policies

## Authorization

There are two authorization approaches implemented in this project:
1. Permission approach (examine if user has required permission). See ReviewController in Shopway.Presentation layer.
2. ApiKey approach (examine if request contains required api key in the "X-Api-Key" header for given endpoint). See ProductController in Shopway.Presentation layer.

## Validator

Defined validator stores all errors up to the moment when Failure is called. Then, the validation result with all errors is created.

## Background jobs by Quartz

Background jobs from Quartz NuGet Package have Scoped lifetime.
Therefore, we can inject scoped services.

In Shopway.App to configure Quartz background jobs we need Quartz.Extensions.Hosting NuGet Package. This integrates with background service in ASP.NET.

## Idempotency

Idempotency means: being able to execute a certain operation multiple times without changing an initial result.

In other words: on retry perform only these operation that were not processed.

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