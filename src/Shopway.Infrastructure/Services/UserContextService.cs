using Microsoft.AspNetCore.Http;
using Shopway.Domain.StronglyTypedIds;
using Shopway.Infrastructure.Abstractions;
using Shopway.Infrastructure.Policies;
using System.Security.Claims;

namespace Shopway.Infrastructure.Services;

//This class is responsible for sharing the information about certain user based on the HTTP Context (so we will be free from strong connection to HttpContext for user data)
//We will be able to get User data in every Service by injecting the IUserContextService
public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public UserId? GetUserId => User is null 
        ? null 
        : UserId.Create(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));

    public PersonId? GetPersonId
    {
        get
        {
            if (User?.FindFirstValue(ClaimPolicies.PersonId) is string stringPersonId)
            {
                return PersonId.Create(Guid.Parse(stringPersonId));
            }

            return null;
        }
    }
}