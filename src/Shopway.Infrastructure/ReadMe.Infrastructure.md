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

## Payments

Used Payment Gateway is my **dummy** application that just reflects the Payment Gateway behavior (like **Stripe** Payment Gateway).

It can be run (with Shopway app) using docker compose (in relese or debug mode). This is the preferred approach, if someone wants to run it without a docker containers, he/she should make some adjustments and configuration changes.

**SecretKey** and **WebhookSecret** should be both stored **securly**, however for demo purposes they are stored in appsettings.json file.

### High level overview

1. We share some secrets with the Payment Gateway provider to be able to confirm the provider's identity.
2. We will simply redirect the customer to the payment gateway provider's website. Therefore, we ask the provider to start the payment session for the current customer and store the payment session information in our database.
3. Payment is processed without any integration or knowledge on our part.
4. Nevertheless, we need be informed by provider about the payment result. Therefore, we configure the webhook (and webhook secret) to which the Payment Gateway provider will send the payment result.
5. We store the informations about the payment result in the database.

### Payment setup:

1. Share **PrivateKey** with Payment Gateway provider. Usually done at the provider's website or in another secure way. Here we use `ShareSecret` endpoint (see postman collection) `/share-secret`. No volume is set, in docker compose, to keep secrets after container destruction. If You want to store them, You can set a volume in docker-compose file.
2. Configure Webhook with Payment Gateway provider. Usually done at providers website. Provider usually allow to set mutliple webhooks (up to 15-20), but here we just allow one webhook. We also configure the **WebhookSecret**. Here we use `ConfigureWebhook` endpoint (see postman collection) `/configure-webhook`. 

### Payment process: 

1. Start payment process for given **OrderHeader** by sending the **Session** details to the Payment Gateway Provider. Here we use `StartPaymentProcess` endpoint (StartPaymentProcessCommand). In **Location** header we send the url for redirection. We could also return the 301 status code, but here we use 200 OK. Copy the **SessionId** and **ClientSecret**.
2. Call (just for demo purpose) the `RedirectToPaymentSession` endpoint and get OK response.
3. Use `PublishPaymentResult_Success` or `PublishPaymentResult_Failure` (paste SessionId and ClientSecret). This will trigger the call to the webhook `FinalizePaymentProcess (Webhook)` endpoint. See in debug.