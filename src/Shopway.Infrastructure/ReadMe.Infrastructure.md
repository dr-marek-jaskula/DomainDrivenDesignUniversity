# Infrastructure Layer :factory:

This layer is responsible for providing the implementations for abstractions for other layers, mostly for .Application layer.
Moreover, this layer contains most tools and additional services. I decided to split database specific implementation to the **.Persistence** layer,
therefore here we have only implementations that are not related to the database.

Therefore, in this project we store:

- Providers
- Adapters
- Decorators
- Validators
- Services
- Policies
- Options
- Outbox pattern

## Validator

Defined validator stores all errors up to the moment when Failure is called. Then, the validation result with all errors is created.

## Idempotency

Idempotency means: being able to execute a certain operation multiple times without changing an initial result.

In other words: on retry perform only these operation that were not processed.

Example: we are publishing the domain event that has 3 event handlers. Lets assume that event handler 1 and 2 execute properly, but event handler 3 fails.
This effects in publishing the domain event again, because of the retry mechanism. Therefore, the event handler 1 and 2 is called again.
If one of this event handlers would deal with payment, the customer would be charged twice.

To overcome this problem we define **OutboxMessageConsumer** class in the Persistence Layer. 
It contains id that matches the id of OutboxMessage and the name, that will come from the event handler, that is being executed.
Thus, we configure this entity to contain the compose key:

```csharp
builder.HasKey(outboxMessageConsumer => new
{
    outboxMessageConsumer.Id,
    outboxMessageConsumer.Name
});
```

Every time we publish the domain event, before handling the actual event, we check in OutboxMessageConsumer if we already processed this message for this event handler.
If so we want to skip it, otherwise we execute the event handler, and we add a new record to the OutboxMessageConsumer table.

In fact, this will be obtain by the use of the Decorator Pattern. We create **IdempotentDomainEventHandlerDecorator** that decorates IDomainEventHandler\<TDomainEvent\> (INotificationHandler\<IDomainEvent\>).

- At first we get the name of the type of the decorated handler (this will be OutboxMessageConsumer.Name)
- Examine if the decorated handler was already processed
	- If so, skip this handler. Otherwise, proceed (handle the domain event).
- After the handler is processed, add to database a new OutboxMessageConsumer with domain event id and handler name.
	- SaveChangesAsync
	
## Outbox pattern

This pattern is used to publish domain events. 

It is useful if we want to ensure that our transaction completes in a anatomic way.

Inside the transaction we generate one or more outbox messages and we save them in the outbox. 
Later, we process the outbox and publish the messages one by one, so they are handled by they respective consumers.

Only AggregateRoots can rise domain events. The IOutboxRepository implementation is in .Persistence project.

The OutboxMessageConsumer is explained in ReadMe.Infrastructur.md chapter **Idempotency**

## FuzzySearch

FuzzySearch is used to approximate strings, spell checking, word segmentation. The use of fuzzy search is presented in 
**.Application/CQRS/Products/Queries/FuzzySearchProductByName**. We should inject the **IFuzzySearchFactory** and then create an
instance of a **FuzzySearch** that matches our expectations.