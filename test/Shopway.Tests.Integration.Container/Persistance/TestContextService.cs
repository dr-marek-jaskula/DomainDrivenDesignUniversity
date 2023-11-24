using System.Security.Claims;
using Shopway.Application.Abstractions;
using static Shopway.Tests.Integration.Container.Constants.Constants;
using Shopway.Domain.Users;

namespace Shopway.Tests.Integration.Persistence;

/// <summary>
/// Test context service, used to set the "CreatedBy" field to the user name
/// </summary>
public sealed class TestContextService : IUserContextService
{
    public ClaimsPrincipal? User => null;

    public UserId? UserId => null;

    public string? Username => TestUser.Username;

    public CustomerId? CustomerId => null;
}