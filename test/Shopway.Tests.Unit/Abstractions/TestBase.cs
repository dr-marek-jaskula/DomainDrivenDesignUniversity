using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Domain.ValueObjects;
using static Shopway.Domain.Utilities.RandomUtilities;

namespace Shopway.Tests.Unit.Abstractions;

public abstract class TestBase
{
    protected readonly string CreatedBy = $"{APP_PREFIX}_{GenerateString(Length)}";
    private const string APP_PREFIX = "auto_shopway";
    private const int Length = 22;
    private const string Name = "TestUser";

    protected static string TestString(int lenght = Length)
    {
        return $"{APP_PREFIX}_test_{GenerateString(lenght)}";
    }

    protected static string Username(string username = Name)
    {
        return username;
    }
}