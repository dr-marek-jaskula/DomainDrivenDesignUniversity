using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.Utilities;
using System.Collections.Frozen;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;

namespace Shopway.Domain.Common.BaseTypes;

/// <summary>
/// DomainEvents are records of something that already occurred in our system.
/// Event names should be in the past.
/// </summary>
/// <param name="Id">DomainEvent id</param>
public abstract record class DomainEvent(Ulid Id) : IDomainEvent
{
    public static readonly FrozenDictionary<Type, Func<IDomainEvent, string>> SerializeCache = Domain.AssemblyReference.Assembly
        .GetTypes()
        .Where(x => x.Implements<IDomainEvent>())
        .Where(x => x.IsAbstract is false && x.IsInterface is false)
        .ToDictionary(keySelector: type => type, elementSelector: domainEventType =>
        {
            var serializeMethod = typeof(DomainEvent)
                .GetSingleGenericMethod(nameof(Serialize), domainEventType);

            return CompileFunc(domainEventType, serializeMethod);
        })
        .ToFrozenDictionary();

    public static string Serialize<TDomainEvent>(IDomainEvent domainEvent)
    {
        return JsonSerializer.Serialize((TDomainEvent)domainEvent);
    }

    private static Func<IDomainEvent, string> CompileFunc(Type eventType, MethodInfo methodInfo)
    {
        var domainEventParameter = Expression.Parameter(typeof(IDomainEvent),nameof(DomainEvent));

        var lambda = Expression.Lambda<Func<IDomainEvent, string>>
        (
            Expression.Call(null, methodInfo, Expression.Convert(domainEventParameter, eventType)),
            false,
            domainEventParameter
        );

        return lambda.Compile();
    }
}

//We rise the domain event by the AggregateRoot, for instance after something has succeeded.
//The DomainEvents are handled using MediatR (they are just a notifications with a ulid Id)
//"<EventName>DomainEventHandler" are consumers of the domain events and they are present in the Application layer
