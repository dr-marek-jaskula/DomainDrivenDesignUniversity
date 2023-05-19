using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.CQRS.Users.Queries.GetUserByUsername;

public sealed record GetUserByUsernameQuery(string Username) : IQuery<UserResponse>;