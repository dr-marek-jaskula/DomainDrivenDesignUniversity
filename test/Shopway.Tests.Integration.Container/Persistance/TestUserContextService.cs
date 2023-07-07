using Shopway.Domain.EntityIds;
using Shopway.Persistence.Abstractions;
using System.Security.Claims;
using Shopway.Tests.Integration.Constants;

namespace Shopway.Tests.Integration.Container.Persistance;

/// <summary>
/// Test context service, used to set the "CreatedBy" field to the user name
/// </summary>
public sealed class TestUserContextService : IUserContextService
{
    public ClaimsPrincipal? User => null;

    public UserId? UserId => null;

    public string? Username => TestUser.Username;

    public CustomerId? CustomerId => null;
}