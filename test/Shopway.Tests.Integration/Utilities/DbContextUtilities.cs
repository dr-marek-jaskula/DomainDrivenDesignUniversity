﻿using Shopway.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using static Shopway.Tests.Integration.Constants.Constants;

namespace Shopway.Tests.Integration.Utilities;

internal static class DbContextUtilities
{
    /// <summary>
    /// Remove records that were created by the TestUser
    /// </summary>
    /// <typeparam name="TAuditableEntity"></typeparam>
    /// <param name="set"></param>
    public static void RemoveTestData<TAuditableEntity>(this DbSet<TAuditableEntity> set)
        where TAuditableEntity : class, IAuditable
    {
        var entities = set.Where(x => x.CreatedBy == TestUser.Username);
        set.RemoveRange(entities);
    }
}