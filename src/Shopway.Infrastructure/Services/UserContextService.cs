using Microsoft.AspNetCore.Http;
using Shopway.Domain.EntityIds;
using Shopway.Domain.Utilities;
using Shopway.Infrastructure.Policies;
using Shopway.Persistence.Abstractions;
using System.Security.Claims;

namespace Shopway.Infrastructure.Services;

//This class is responsible for sharing the information about certain user based on the HTTP Context (so we will be free from strong connection to HttpContext for user data)
//We will be able to get User data in every Service by injecting the IUserContextService
public sealed class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public UserId? UserId => User is null 
        ? null 
        : global::Shopway.Domain.EntityIds.UserId.Create(global::System.Guid.Parse(User.FindFirstValue(global::System.Security.Claims.ClaimTypes.NameIdentifier)!));

    public string? Username => User?.FindFirstValue("name");

    public PersonId? PersonId
    {
        get
        {
            if (User?.FindFirstValue(ClaimPolicies.PersonId) is string stringPersonId && stringPersonId.IsNullOrEmptyOrWhiteSpace() == false)
            {
                return Domain.EntityIds.PersonId.Create(Guid.Parse(stringPersonId));
            }

            return null;
        }
    }
}