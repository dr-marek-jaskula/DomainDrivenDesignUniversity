using Microsoft.EntityFrameworkCore;
using Shopway.Domain.BaseTypes;
using Shopway.Domain.Entities;
using Shopway.Domain.Entities.Parents;
using Shopway.Domain.EntityIds;
using Shopway.Domain.ValueObjects;
using Shopway.Persistence.Framework;
using Shopway.Tests.Integration.Utilities;
using static Shopway.Domain.Utilities.RandomUtilities;

namespace Shopway.Tests.Integration.Persistance;

public class TestDataGenerator
{
    private readonly IUnitOfWork<ShopwayDbContext> _unitOfWork;
    private const string AUTO_PREFIX = "auto";
    private const int Length = 22;
    private readonly Random _random = new();

    public TestDataGenerator(IUnitOfWork<ShopwayDbContext> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    internal async Task CleanupDatabase()
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

    public static string TestStringWithPrefix(int lenght = Length)
    {
        if (lenght - AUTO_PREFIX.Length <= 0)
        {
            throw new ArgumentException($"{lenght} must be greater than AUTO_PREFIX length");
        }

        return $"{AUTO_PREFIX}{GenerateString(lenght - AUTO_PREFIX.Length)}";
    }

    public static string TestString(int lenght = Length)
    {
        return $"{GenerateString(lenght)}";
    }

    public static string NotTrimmedTestString(int lenght = Length)
    {
        return $" \n  \t \n    \t {GenerateString(lenght)}  \n \t   \n ";
    }

    public int TestInt(int min = 1, int max = 1000)
    {
        return _random.Next(min, max);
    }

    public async Task<ProductId> AddProductWithoutReviews
    (
        ProductName? productName = null,
        Revision? revision = null,
        Price? price = null,
        UomCode? uomCode = null
    )
    {
        productName ??= ProductName.Create(TestString(20)).Value;
        price ??= Price.Create(TestInt(1, 10)).Value;
        uomCode ??= UomCode.Create(UomCode.AllowedUomCodes.ElementAt(_random.Next(0, UomCode.AllowedUomCodes.Length - 1))).Value;
        revision ??= Revision.Create(TestString(2)).Value;

        var product = Product.Create
        (
            id: ProductId.New(),
            productName: productName,
            price: price,
            uomCode: uomCode,
            revision: revision
        );

        await AddEntity(product);

        return product
            .Id;
    }

    private async Task AddEntity<TEntity>(TEntity entity)
        where TEntity : class, IEntity
    {
        _unitOfWork.Context
            .Set<TEntity>()
            .Add(entity);

        await _unitOfWork.SaveChangesAsync();
    }
}