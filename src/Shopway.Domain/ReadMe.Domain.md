# Domain Layer :anchor:

In this layer we define: 

- Entities (and entities ids)
- Aggregates
- Value Objects
- Enumerations
- Domain Events
- Errors
- Enums
- Results
- Constants
- Common components
- IRepositories
- Interfaces and utilities of generic purpose

## Validation

Validation in the Domain Layer is mainly connected to ValueObjects. 

Errors, that occur when in-domain validation fails, need to be explicitly handled. This is done by "Validator" that is defined in .Infrastructure

We can examine handling the in-domain validation in for instance "CreateProductCommandHandler.Handle"

## Ulid instead of Guid

Ulid is a Universally Unique Lexicographically Sortable Identifier. 

The biggest advantage of Ulid is that is it Lexicographically sortable. This is very important feature, because it allows 
to use cursor pagination and therefore significantly increase the performance of some queries.

Other advantages of ulid:
- Canonically encoded as a 26 character string
- Uses Crockford's base32 for better efficiency and readability (5 bits per character)
- Case insensitive
- No special characters (URL safe)
- Monotonic sort order (correctly detects and handles the same millisecond)

Moreover, we use [Ulid](https://github.com/Cysharp/Ulid) NuGet Package that provides more efficient operation on Uilds than similar ones on Guids.

## EntityIds and EntityIdConverter

EntityId is a strongly typed id that is a readonly record struct (due to the fact that ulid is a struct).

The EntityIdConverter handles the conversion from and to string, so we can have controllers parameters like "PersonId",
because the conversion will be done behind the scenes. If conversion fails, the proper error message is returned.

To create a new id we use a static method "New". For instance, "ProductId.New()".

To create a strongly typed id based on a given ulid, we use "Create" method. For instance, "Product.Create(ulid)".

Due to the fact that entity id is a record struct, we can use the '==' operator to compare ids.

Ulid base EntityId allows the comparing two entities. Therefore, the IComparable interface is implemented for IEntityId. Nevertheless,
overriding the '<', '>', '>=', '<=' operator should be done both in the interface and for each concrete EntityId (for instance ProductId).

## Domain Event

"A Domain Event captures the memory of something interesting which affects the domain" — Martin Fowler

Domain event should not leave the bounded context. Therefore, we can use the domain specific language - for instance we can use strongly typed ids.
