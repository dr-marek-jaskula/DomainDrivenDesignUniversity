using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Entities;
using Shopway.Domain.Entities.Parents;
using Shopway.Persistence.Abstractions;
using Shopway.Persistence.Framework;
using Shopway.Tests.Integration.Utilities;
using static Shopway.Domain.Utilities.RandomUtilities;

namespace Shopway.Tests.Integration.Abstractions;

public abstract class TestDataGeneratorBase
{
    protected readonly IUnitOfWork<ShopwayDbContext> _unitOfWork;
    protected const string AUTO_PREFIX = "auto";
    protected const int Length = 22;
    protected readonly Random _random = new();

    public TestDataGeneratorBase(IUnitOfWork<ShopwayDbContext> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Generates test string with 'auto' prefix
    /// </summary>
    /// <param name="lenght">Must be greater than 4</param>
    /// <returns>Test string with prefix</returns>
    /// <exception cref="ArgumentException"></exception>
    public static string TestStringWithPrefix(int lenght = Length)
    {
        if (lenght - AUTO_PREFIX.Length <= 0)
        {
            throw new ArgumentException($"{lenght} must be greater than AUTO_PREFIX length");
        }

        return $"{AUTO_PREFIX}{GenerateString(lenght - AUTO_PREFIX.Length)}";
    }

    /// <summary>
    /// Generates test string with given length
    /// </summary>
    /// <param name="lenght">String length</param>
    /// <returns>Test string</returns>
    public static string TestString(int lenght = Length)
    {
        return $"{GenerateString(lenght)}";
    }

    /// <summary>
    /// Generates not trimmed test string
    /// </summary>
    /// <param name="length">The length of string (not including spaces)</param>
    /// <returns>Test string with enter, tabs and white spaces from both sides</returns>
    public static string NotTrimmedTestString(int length = Length)
    {
        return $" \n  \t \n    \t {GenerateString(length)}  \n \t   \n ";
    }

    /// <summary>
    /// Generates test int in given range
    /// </summary>
    /// <param name="min">Lower bound</param>
    /// <param name="max">Upper bound</param>
    /// <returns></returns>
    public int TestInt(int min = 1, int max = 1000)
    {
        return _random.Next(min, max);
    }

    /// <summary>
    /// Clean the database from the test data
    /// </summary>
    internal async Task CleanDatabaseFromTestData()
    {
        foreach (var entityEntry in _unitOfWork.Context.ChangeTracker.Entries())
        {
            await entityEntry.ReloadAsync();
        }

        _unitOfWork.Context.Set<Product>().RemoveTestData();
        _unitOfWork.Context.Set<Review>().RemoveTestData();
        _unitOfWork.Context.Set<Person>().RemoveTestData();
        _unitOfWork.Context.Set<User>().RemoveTestData();
        _unitOfWork.Context.Set<Order>().RemoveTestData();
        _unitOfWork.Context.Set<Payment>().RemoveTestData();
        _unitOfWork.Context.Set<WorkItem>().RemoveTestData();

        _unitOfWork.Context.Database.SetCommandTimeout(TimeSpan.FromMinutes(2));

        try
        {
            await _unitOfWork.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException exception)
        {
            foreach (var entry in exception.Entries)
            {
                entry.State = EntityState.Detached;
            }

            await _unitOfWork.SaveChangesAsync();
        }

        _unitOfWork.Context.ChangeTracker.Clear();
    }

    /// <summary>
    /// Insert the entity to the database
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <param name="entity">Entity to be inserted</param>
    protected async Task AddEntity<TEntity>(TEntity entity)
        where TEntity : class, IEntity
    {
        _unitOfWork.Context
            .Set<TEntity>()
            .Add(entity);

        await _unitOfWork.SaveChangesAsync();
    }
}