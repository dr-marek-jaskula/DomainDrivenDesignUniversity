using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Users.Commands.RemoveRoleFromUser;

public sealed record RemoveRoleFromUserCommand(string Username, string Role) : ICommand;
