# Application Layer

Application layer contains:

- Abstractions/Interfaces
- Application Services/Handlers
- Commands, Queries and Events + Validators
- Responses (sometimes called DTOs or Dtos)
- Mappers (do not use AutoMapper, use custom made mappers instead like in this project)
- Pipelines

## Validators

Validators in the Application Layer should mostly deal with the combinations of the request parameters. 

Example:

```csharp
RuleFor(m => new {m.CountyId, m.Zip}).Must(x => ValidZipCounty(x.Zip, x.CountyId))
```

This is caused by the fact that the in-domain validation will handle the remaining validation.

The other validation cases are to validate request guids that are not translated to ValueObjets (other way is to have ValueObjects like ProductId)