# Application Layer

In this layer we define: 

- CQRS abstraction and implementation
- CQRS validation
- pagination
- responses (sometimes called "DTOs" or "Dtos")
- mappings (do not use AutoMapper, use custom made mappers instead!)
- request pipelines

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

3. The remaining validation is done in the domain or in presentation layer (model validation)

## Transaction Pipelines

There is not need to save changes inside handlers, because it is done in the transaction pipeline.
Moreover, the pipeline provides the transaction scope, so if at least one database operation fail, all operations are rollbacked.
Therefore in some cases, we return a task, which slightly increases the performance.