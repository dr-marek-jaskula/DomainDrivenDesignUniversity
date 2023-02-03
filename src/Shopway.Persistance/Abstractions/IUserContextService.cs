using Shopway.Domain.EntityIds;
using System.Security.Claims;

namespace Shopway.Persistence.Abstractions;

public interface IUserContextService
{
    public ClaimsPrincipal? User { get; }
    public UserId? UserId { get; }
    public PersonId? PersonId { get; }
    public string? Username { get; }
}
