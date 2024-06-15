using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Users.Queries.GetUserRoles;

public sealed record GetUserRolesByUsernameQuery(string Username) : IQuery<RolesResponse>;
