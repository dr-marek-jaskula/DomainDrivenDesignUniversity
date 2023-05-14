using Microsoft.AspNetCore.Http;
using Shopway.Domain.EntityIds;
using Shopway.Domain.Utilities;
using Shopway.Infrastructure.Policies;
using Shopway.Persistence.Abstractions;
using System.Security.Claims;
using System.Threading;

namespace Shopway.Infrastructure.Services;

/// <summary>
/// Responsible for sharing informations about user based on the HTTP Context.
/// Therefore, we avoid the strong connection to HttpContext for user data
/// We will be able to get User data in every Service by injecting the IUserContextService
/// </summary>
public sealed class UserContextService : IUserContextService
{
    // It is recommended to retrieve the HttoContext from the IHttpContextAccessor when it is needed, and use it only within the scope of the method or block of code that requires it.
    //This way, you can ensure that each thread gets its own instance of the HttpContext object, and that it is not shared across threads.
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

    public CustomerId? CustomerId
    {
        get
        {
            if (User?.FindFirstValue(ClaimPolicies.CustomerId) is string stringCustomerId && stringCustomerId.NotNullOrEmptyOrWhiteSpace())
            {
                return Domain.EntityIds.CustomerId.Create(Guid.Parse(stringCustomerId));
            }

            return null;
        }
    }
}