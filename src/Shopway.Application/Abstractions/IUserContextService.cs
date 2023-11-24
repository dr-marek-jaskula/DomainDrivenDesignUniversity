using Shopway.Domain.Users;
using System.Security.Claims;

namespace Shopway.Application.Abstractions;

public interface IUserContextService
{
    ClaimsPrincipal? User { get; }
    UserId? UserId { get; }
    CustomerId? CustomerId { get; }
    string? Username { get; }
}
