using Shopway.Domain.EntityIds;
using System.Security.Claims;

namespace Shopway.Persistence.Abstractions;

public interface IUserContextService
{
    ClaimsPrincipal? User { get; }
    UserId? UserId { get; }
    CustomerId? CustomerId { get; }
    string? Username { get; }
}
