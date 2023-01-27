using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Domain.ValueObjects;
using Shopway.Persistence.Framework;
using static Shopway.Domain.Utilities.RandomUtilities;

namespace Shopway.Tests.Integration.Persistance;

public class TestDataGenerator
{
    private readonly ShopwayDbContext _context;
    private const string AUTO_PREFIX = "auto_";
    private const int Length = 22;
    private readonly Random _random = new();

    public TestDataGenerator(ShopwayDbContext context)
    {
        _context = context;
    }

    public async Task ClearDatabase()
    {
        //Disable all constraints
        //Delete data in all tables (but not from __EFMigrationsHistory) and use SET QUOTED_IDENTIFIES ON -> (QUOTED_IDENTIFIER controls the behavior of SQL Server handling double-quotes)
        //Enable all constraints 
        //Reseed identity columns if the table has identity
        await _context.Database.ExecuteSqlRawAsync(@"
            EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT all';
            EXEC sp_MSForEachTable @command1 = 'SET QUOTED_IDENTIFIER ON; DELETE FROM ?', @whereand = ' AND Object_id IN (SELECT Object_id FROM sys.objects WHERE name != ''__EFMigrationsHistory'')';
            EXEC sp_MSForEachTable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all';
            EXEC sp_MSForEachTable 'IF (OBJECTPROPERTY(OBJECT_ID(''?''), ''TableHasIdentity'') = 1) DBCC CHECKIDENT (''?'', RESEED, 0)';
            ");

        _context.ChangeTracker.Clear();
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

        _context
            .Set<Product>()
            .Add(product);

        await _context.SaveChangesAsync();

        return product
            .Id;
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

    public int TestInt(int min = 1, int max = 1000)
    {
        return _random.Next(min, max);
    }
}