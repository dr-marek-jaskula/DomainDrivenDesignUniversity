using Shopway.Domain.Abstractions;
using Shopway.Persistence.Abstractions;
using Shopway.Persistence.Framework;
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
    /// <param name="length">Must be greater than 4</param>
    /// <returns>Test string with prefix</returns>
    /// <exception cref="ArgumentException"></exception>
    public static string TestStringWithPrefix(int length = Length)
    {
        if (length - AUTO_PREFIX.Length <= 0)
        {
            throw new ArgumentException($"{length} must be greater than {nameof(AUTO_PREFIX)} length: {AUTO_PREFIX.Length}");
        }

        return $"{AUTO_PREFIX}{GenerateString(length - AUTO_PREFIX.Length)}";
    }

    /// <summary>
    /// Generates test string with given length
    /// </summary>
    /// <param name="length">String length</param>
    /// <returns>Test string</returns>
    public static string TestString(int length = Length)
    {
        return $"{GenerateString(length)}";
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
    /// Insert the entity to the database
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <param name="entity">Entity to be inserted</param>
    protected async Task AddEntityAsync<TEntity>(TEntity entity)
        where TEntity : class, IEntity
    {
        _unitOfWork.Context
            .Set<TEntity>()
            .Add(entity);

        await _unitOfWork.SaveChangesAsync(CancellationToken.None);
    }
}