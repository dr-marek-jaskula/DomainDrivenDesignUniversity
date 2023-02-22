using Microsoft.AspNetCore.Http;
using Shopway.Domain.EntityIds;
using Shopway.Domain.Utilities;
using Shopway.Infrastructure.Policies;
using Shopway.Persistence.Abstractions;
using System.Security.Claims;

namespace Shopway.Infrastructure.Services;

/// <summary>
/// Responsible for sharing informations about user based on the HTTP Context.
/// Therefore, we avoid the strong connection to HttpContext for user data
/// We will be able to get User data in every Service by injecting the IUserContextService
/// </summary>
public sealed class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string _userNameProperty = "name";

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public UserId? UserId => User is null 
        ? null 
        : Shopway.Domain.EntityIds.UserId.Create(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));

    public string? Username => User?.FindFirstValue(_userNameProperty);

    public PersonId? PersonId
    {
        get
        {
            if (User?.FindFirstValue(ClaimPolicies.PersonId) is string stringPersonId && stringPersonId.IsNullOrEmptyOrWhiteSpace() is false)
            {
                return Domain.EntityIds.PersonId.Create(Guid.Parse(stringPersonId));
            }

            return null;
        }
    }
}