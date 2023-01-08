using Shopway.Domain.StronglyTypedIds;
using System.Security.Claims;

namespace Shopway.Infrastructure.Abstractions;

public interface IUserContextService
{
    public ClaimsPrincipal? User { get; }
    public UserId? GetUserId { get; }
    public PersonId? GetPersonId { get; }
}
