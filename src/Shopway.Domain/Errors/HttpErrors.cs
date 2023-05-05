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
        return new Error($"{typeof(TEntity).Name}.{nameof(NotFound)}", $"{typeof(TEntity).Name} with Id: '{id.Value}' was not found");
    }

    /// <summary>
    /// Create an Error based on the business key
    /// </summary>
    /// <param name="key">business key of the entity that is already in the database</param>
    /// <returns>AlreadyExists error</returns>
    public static Error AlreadyExists<TBusinessKey>(TBusinessKey key)
        where TBusinessKey : IBusinessKey
    {
        return new Error($"{typeof(TBusinessKey).Name}.{nameof(AlreadyExists)}", $"{typeof(TBusinessKey).Name} with key: '{key}' already exists");
    }

    /// <summary>
    /// Create an Error describing that the provided reference is invalid
    /// </summary>
    /// <returns>InvalidReference error</returns>
    public static Error InvalidReference(Guid reference, string entity)
    {
        return new Error($"{nameof(Error)}.{nameof(InvalidReference)}", $"Invalid reference {reference} for entity {entity}");
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
        where TBusinessKey : IBusinessKey
    {
        return new Error($"{nameof(Error)}.{nameof(DuplicatedRequest)}", $"Duplicated request for key {key}");
    }

    /// <summary>
    /// Create an Error from the thrown exception
    /// </summary>
    /// <param name="exceptionMessage">Exception message</param>
    /// <returns>Exception error</returns>
    public static Error Exception(string exceptionMessage)
    {
        return new Error($"{nameof(Error)}.{nameof(Exception)}", exceptionMessage);
    }
}
