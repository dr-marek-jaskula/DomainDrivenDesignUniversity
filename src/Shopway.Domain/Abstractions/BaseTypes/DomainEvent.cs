using MediatR;

namespace Shopway.Domain.Abstractions.BaseTypes;

public interface IDomainEvent : INotification
{
    public Guid Id { get; init; }
}

//DomainEvents are records of something that already occurred in our system
//For instance: UserCreatedDomainEvent, OrderCreatedDomainEvent, PaymentSucceededDomainEvent
//Event names should be in the past
//Preferred way: use records for domain events
public abstract record class DomainEvent(Guid Id) : IDomainEvent;

//We rise the domain event in the Entity methods, for instance after something has succeeded.
//The DomainEvents are handled using MediatR (they are just a notifications with a guid Id)
//"<EventName>DomainEventHandler" is present in the Application layer, in Events folder
//(Shop.Application -> Events)