using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Abstractions;
using Shopway.Tests.Integration.Constants;

namespace Shopway.Tests.Integration.Utilities;

internal static class DbContextUtilities
{
    public static void RemoveTestData<TAuditableEntity>(this DbSet<TAuditableEntity> set)
        where TAuditableEntity : class, IAuditableEntity
    {
        var entities = set.Where(x => x.CreatedBy == TestUser.Username);
        set.RemoveRange(entities);
    }
}