using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Users.Commands.AddRoleToUser;

public sealed record AddRoleToUserCommand(string Username, string Role) : ICommand;
