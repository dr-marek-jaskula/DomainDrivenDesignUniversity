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

## EntityIds and EntityIdConverter

EntityId is a strongly typed id that is a readonly record struct (due to the fact that guid is a struct).

The EntityIdConverter handles the conversion from and to string, so we can have controllers parameters like "PersonId",
because the conversion will be done behind the scenes. If conversion fails, the proper error message is returned.

To create a new id we use a static method "New". For instance, "ProductId.New()".

To create a strongly typed id based on a given guid, we use "Create" method. For instance, "Product.Create()".

Due to the fact that entity id is a record struct, we can use the '==' operator to compare ids.

## Domain Event

"A Domain Event captures the memory of something interesting which affects the domain" — Martin Fowler

Domain event should not leave the bounded context. Therefore, we can use the domain specific language - for instance we can use strongly typed ids.
