using System.Security.Claims;
using Shopway.Domain.EntityIds;

namespace Shopway.Persistence.Abstractions;

public interface IUserContextService
{
    ClaimsPrincipal? User { get; }
    UserId? UserId { get; }
    CustomerId? CustomerId { get; }
    string? Username { get; }
}
