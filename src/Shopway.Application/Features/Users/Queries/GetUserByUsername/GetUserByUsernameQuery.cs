using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Users.Queries.GetUserByUsername;

public sealed record GetUserByUsernameQuery(string Username) : IQuery<UserResponse>;