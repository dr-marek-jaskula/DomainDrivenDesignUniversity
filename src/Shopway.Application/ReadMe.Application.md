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

## Mapping 

Manual mapping is way faster than mapping done by external libraries like automapper. Therefore, manual mapping
is done in "Mapping" folder.

Nevertheless, it is important to examine sql queries that are created for different types of mappings:

For instance, when in **ProductPageQueryHandler** we would use 
```csharp
        var pageResponse = await queryable
            .ToPageResponse(pageQuery.PageSize, pageQuery.PageNumber, product => product.ToResponse(), cancellationToken);
```

We would get

```sql
DECLARE @__ProductName_0 nvarchar(50) = N'Bike';
DECLARE @__p_1 int = 0;
DECLARE @__p_2 int = 15;

SELECT [t].[Id], [t].[CreatedBy], [t].[CreatedOn], [t].[UpdatedBy], [t].[UpdatedOn], [t].[Price], [t].[ProductName], [t].[Revision], [t].[UomCode], [r].[Id], [r].[CreatedBy], [r].[CreatedOn], [r].[ProductId], [r].[UpdatedBy], [r].[UpdatedOn], [r].[Description], [r].[Stars], [r].[Title], [r].[Username]
FROM (
    SELECT [p].[Id], [p].[CreatedBy], [p].[CreatedOn], [p].[UpdatedBy], [p].[UpdatedOn], [p].[Price], [p].[ProductName], [p].[Revision], [p].[UomCode]
    FROM [Shopway].[Product] AS [p]
    WHERE (@__ProductName_0 LIKE N'') OR CHARINDEX(@__ProductName_0, [p].[ProductName]) > 0
    ORDER BY [p].[ProductName], [p].[Price] DESC
    OFFSET @__p_1 ROWS FETCH NEXT @__p_2 ROWS ONLY
) AS [t]
LEFT JOIN [Shopway].[Review] AS [r] ON [t].[Id] = [r].[ProductId]
ORDER BY [t].[ProductName], [t].[Price] DESC, [t].[Id]
```

So we would query redundant data. This is caused by using the extension method ToResponse() that entity framework is not able to translate, so it queries all data.

However, if we use 
```csharp
        var pageResponse = await queryable
            .ToPageResponse(pageQuery.PageSize, pageQuery.PageNumber, ProductMapping.ToResponse(), cancellationToken);
```

We would get

```sql
SELECT [t].[Id], [t].[ProductName], [t].[Revision], [t].[Price], [t].[UomCode], [r].[Id], [r].[Username], [r].[Stars], [r].[Title], [r].[Description]
FROM (
    SELECT [p].[Id], [p].[ProductName], [p].[Revision], [p].[Price], [p].[UomCode]
    FROM [Shopway].[Product] AS [p]
    WHERE (@__ProductName_0 LIKE N'') OR CHARINDEX(@__ProductName_0, [p].[ProductName]) > 0
    ORDER BY [p].[ProductName], [p].[Price] DESC
    OFFSET @__p_1 ROWS FETCH NEXT @__p_2 ROWS ONLY
) AS [t]
LEFT JOIN [Shopway].[Review] AS [r] ON [t].[Id] = [r].[ProductId]
ORDER BY [t].[ProductName], [t].[Price] DESC, [t].[Id]
```

which is a way better. Here we pass the expression that does not use any method that entity framework cannot translate.

Same goes for **ProductDictionaryPageQueryHandler**.