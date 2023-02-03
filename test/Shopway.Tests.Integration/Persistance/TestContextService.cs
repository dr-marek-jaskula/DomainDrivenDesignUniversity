using Microsoft.AspNetCore.Http;
using Shopway.Domain.EntityIds;
using Shopway.Domain.Utilities;
using Shopway.Infrastructure.Policies;
using Shopway.Persistence.Abstractions;
using System.Security.Claims;

namespace Shopway.Tests.Integration.Persistance;

public sealed class TestContextService : IUserContextService
{
    public ClaimsPrincipal? User => null;

    public UserId? UserId => null;

    public string? Username => TestDataGenerator.TestStringWithPrefix();

    public PersonId? PersonId => null;
}