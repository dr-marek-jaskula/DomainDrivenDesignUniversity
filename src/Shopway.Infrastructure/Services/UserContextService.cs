using Shopway.Domain.Users;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Shopway.Domain.Common.Utilities;
using Shopway.Infrastructure.Policies;
using Shopway.Application.Abstractions;

namespace Shopway.Infrastructure.Services;

/// <summary>
/// Responsible for sharing informations about user based on the HTTP Context.
/// Therefore, we avoid the strong connection to HttpContext for user data
/// We will be able to get User data in every Service by injecting the IUserContextService
/// </summary>
public sealed class UserContextService(IHttpContextAccessor httpContextAccessor) : IUserContextService
{
    // It is recommended to retrieve the HttoContext from the IHttpContextAccessor when it is needed, and use it only within the scope of the method or block of code that requires it.
    //This way, you can ensure that each thread gets its own instance of the HttpContext object, and that it is not shared across threads.
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private const string _userNameProperty = "name";

    public ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public UserId? UserId => User is null 
        ? null 
        : Shopway.Domain.Users.UserId.Create(Ulid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));

    public string? Username => User?.FindFirstValue(_userNameProperty);

    public CustomerId? CustomerId
    {
        get
        {
            if (User?.FindFirstValue(ClaimPolicies.CustomerId) is string stringCustomerId && stringCustomerId.NotNullOrEmptyOrWhiteSpace())
            {
                return Domain.Users.CustomerId.Create(Ulid.Parse(stringCustomerId));
            }

            return null;
        }
    }
}