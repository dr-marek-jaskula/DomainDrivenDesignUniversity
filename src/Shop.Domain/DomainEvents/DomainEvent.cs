using Shopway.Domain.Primitives;

namespace Shopway.Domain.DomainEvents;

//DomainEvents are record of something that already occurred in our system
//For instanceL UserCreatedDomainEvent, OrderCreatedDomainEvent, PaymentSucceededDomainEvent
//Names of the events must be in the past
//Preferred way: use record for domain events
public abstract record DomainEvent(Guid Id) : IDomainEvent;

//We rise the domain events in the Entities methods, for instance after something has succeeded.
//The DomainEvents are handled using MediatR (they are just a notifications with a guid Id)
//"<EventName>DomainEventHandler" is present in the Application layer in Events folder
//(Shop.Application -> Events)