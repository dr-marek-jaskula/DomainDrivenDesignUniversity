using Shopway.Domain.EntityIds;
using System.Security.Claims;

namespace Shopway.Application.Abstractions;

public interface IUserContextService
{
    public ClaimsPrincipal? User { get; }
    public UserId? GetUserId { get; }
    public PersonId? GetPersonId { get; }
    public string? GetUserName { get; }
}
