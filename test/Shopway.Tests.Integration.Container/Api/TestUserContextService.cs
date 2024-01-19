using Shopway.Application.Abstractions;
using Shopway.Domain.Users;
using System.Security.Claims;
using static Shopway.Tests.Integration.Container.Constants.Constants;

namespace Shopway.Tests.Integration.Container.Api;

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