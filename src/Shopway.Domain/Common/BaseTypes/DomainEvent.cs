using Shopway.Domain.Common.BaseTypes.Abstractions;

namespace Shopway.Domain.Common.BaseTypes;

/// <summary>
/// DomainEvents are records of something that already occurred in our system.
/// Event names should be in the past.
/// </summary>
/// <param name="Id">DomainEvent id</param>
public abstract record class DomainEvent(Ulid Id) : IDomainEvent;

//We rise the domain event by the AggregateRoot, for instance after something has succeeded.
//The DomainEvents are handled using MediatR (they are just a notifications with a ulid Id)
//"<EventName>DomainEventHandler" are consumers of the domain events and they are present in the Application layer
