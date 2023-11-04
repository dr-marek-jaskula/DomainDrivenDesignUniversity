using System.Security.Claims;
using Shopway.Domain.EntityIds;

namespace Shopway.Application.Abstractions;

public interface IUserContextService
{
    ClaimsPrincipal? User { get; }
    UserId? UserId { get; }
    CustomerId? CustomerId { get; }
    string? Username { get; }
}
