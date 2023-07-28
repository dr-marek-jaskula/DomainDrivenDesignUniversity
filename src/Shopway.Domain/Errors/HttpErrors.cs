using Shopway.Domain.Abstractions;
using Shopway.Domain.Entities;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Errors;

public static class HttpErrors
{
    /// <summary>
    /// Create an Error describing that a password or an email are invalid
    /// </summary>
    public static readonly Error InvalidPasswordOrEmail = new($"{nameof(User)}.{nameof(InvalidPasswordOrEmail)}", $"Invalid {nameof(Password)} or {nameof(Email)}");

    /// <summary>
    /// Create an Error based on the entity type name and the id that was not found
    /// </summary>
    /// <param name="name">name of the entity type. Use "nameof(TValue)" syntax</param>
    /// <param name="id">id of the entity that was not found</param>
    /// <returns>NotFound error</returns>
    public static Error NotFound<TEntity>(IEntityId<TEntity> id)
        where TEntity : class, IEntity
    {
        return new($"{typeof(TEntity).Name}.{nameof(NotFound)}", $"{typeof(TEntity).Name} with Id: '{id.Value}' was not found");
    }

    /// <summary>
    /// Create an NotFount Error based on the entity type name and the unique key
    /// </summary>
    /// <param name="name">name of the entity type. Use "nameof(TValue)" syntax</param>
    /// <param name="key">Key of the entity that was not found</param>
    /// <returns>NotFound error</returns>
    public static Error NotFound<TEntity>(IUniqueKey key)
        where TEntity : class, IEntity
    {
        return new($"{typeof(TEntity).Name}.{nameof(NotFound)}", $"{typeof(TEntity).Name} with Key: '{key}' was not found");
    }

    /// <summary>
    /// Create an NotFound Error based on the entity type name and the unique value
    /// </summary>
    /// <param name="name">name of the entity type. Use "nameof(TValue)" syntax</param>
    /// <param name="uniqueValue">unique value of the entity that was not found</param>
    /// <returns>NotFound error</returns>
    public static Error NotFound<TEntity>(string uniqueValue)
        where TEntity : class, IEntity
    {
        return new($"{typeof(TEntity).Name}.{nameof(NotFound)}", $"{typeof(TEntity).Name} for: '{uniqueValue}' was not found");
    }

    /// <summary>
    /// Create an Error based on the unique key
    /// </summary>
    /// <param name="key">unique key key of the entity that is already in the database</param>
    /// <returns>AlreadyExists error</returns>
    public static Error AlreadyExists<TUniqueKey>(TUniqueKey key)
        where TUniqueKey : IUniqueKey
    {
        return new($"{typeof(TUniqueKey).Name}.{nameof(AlreadyExists)}", $"{typeof(TUniqueKey).Name} with key: '{key}' already exists");
    }

    /// <summary>
    /// Create an Error describing that the provided reference is invalid
    /// </summary>
    /// <returns>InvalidReference error</returns>
    public static Error InvalidReference(Ulid reference, string entity)
    {
        return new($"{nameof(Error)}.{nameof(InvalidReference)}", $"Invalid reference {reference} for entity {entity}");
    }

    /// <summary>
    /// Create an Error describing that the provided reference is invalid
    /// </summary>
    /// <returns>InvalidReference error</returns>
    public static Error InvalidReferences(IList<Ulid> references, string entity)
    {
        return new($"{nameof(Error)}.{nameof(InvalidReference)}", $"Invalid references: [{string.Join(", ", references)}] for entity {entity}");
    }

    /// <summary>
    /// Create an Error describing that the collection is null or empty
    /// </summary>
    /// <returns>NullOrEmpty error</returns>
    public static Error NullOrEmpty(string collectionName)
    {
        return new($"{nameof(Error)}.{nameof(NullOrEmpty)}", $"{collectionName} is null or empty");
    }

    /// <summary>
    /// Create an Error describing that the collection is null or empty
    /// </summary>
    /// <returns>InvalidBatchCommand error</returns>
    public static Error InvalidBatchCommand(string batchCommand)
    {
        return new($"{nameof(Error)}.{nameof(InvalidBatchCommand)}", $"{batchCommand} is invalid");
    }

    /// <summary>
    /// Create an Error describing that the request is duplicated (same key)
    /// </summary>
    /// <returns>DuplicatedRequest error</returns>
    public static Error DuplicatedRequest<TBusinessKey>(TBusinessKey key)
        where TBusinessKey : IUniqueKey
    {
        return new($"{nameof(Error)}.{nameof(DuplicatedRequest)}", $"Duplicated request for key {key}");
    }

    /// <summary>
    /// Create an Error from the thrown exception
    /// </summary>
    /// <param name="exceptionMessage">Exception message</param>
    /// <returns>Exception error</returns>
    public static Error Exception(string exceptionMessage)
    {
        return new($"{nameof(Error)}.{nameof(Exception)}", exceptionMessage);
    }

    /// <summary>
    /// Create an Error describing that value was not parsed properly
    /// </summary>
    /// <param name="errorMessage">Exception message</param>
    /// <returns>Exception error</returns>
    public static Error ParseFailure<ParseType>(string valueParsedName)
    {
        return new($"{nameof(Error)}.{nameof(ParseFailure)}", $"Parsing {valueParsedName} to type {nameof(ParseType)} failed");
    }
}
