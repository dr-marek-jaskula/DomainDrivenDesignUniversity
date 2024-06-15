using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Users;

namespace Shopway.Application.Features.Users.Commands.Revoke;

public sealed record RevokeRefreshTokenCommand(UserId UserId) : ICommand;
