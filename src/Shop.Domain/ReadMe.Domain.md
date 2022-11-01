# Domain Layer

- Entities
- Aggregates
- Value Objects
- Domain Events
- Erros
- Enums
- Results
- Constants
- IRepositories

## Validation

Validation in the Domain Layer is for particular ValueObjects. 

Errors that occur when in-domain validation fails need to be explicitly handled. For this purpose, we introduce the ErrorHandler.

We can examine handling the in-domain validation in for instance "RemoveProductCommandHandler.Handle"