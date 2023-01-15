# Application Layer

Application layer contains:

- Abstractions/Interfaces
- Application Services/Handlers
- Commands, Queries and Events + Validators
- Paginataion (filters and order)
- Responses (sometimes called DTOs or Dtos)
- Mappers (do not use AutoMapper, use custom made mappers instead like in this project)
- Pipelines

## Validators

1. FleuntValidator
	Mostly deal with the combinations of the request parameters. 

Example:

```csharp
RuleFor(m => new {m.CountyId, m.Zip}).Must(x => ValidZipCounty(x.Zip, x.CountyId))
```

Or see **ProductPageQueryValidator**

2. ReferenceValidation
	MediatR pipeline that checks if the given id truly refers to the entity
	Therefore, verifying that the entity is not null in the handler is redundant.

3. The remaining validation is done in the domain.

## Transaction Pipelines

There is not need to save changes inside handlers, because is done in the Pipeline (transaction pipeline)
Moreover, the pipeline provides the transaction scope, so if at least one database operation fail, all operations are rollbacked.
Therefore in some cases, we return a task, which slightly increases the performance 