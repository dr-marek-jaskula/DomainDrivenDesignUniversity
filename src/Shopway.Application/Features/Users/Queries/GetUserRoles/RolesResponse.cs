using Shopway.Application.Abstractions;

namespace Shopway.Application.Features.Users.Queries.GetUserRoles;

public sealed record RolesResponse(List<string> Roles) : IResponse;