using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Users.Commands.DeletePermission;

public sealed record DeletePermissionCommand(string PermissionName) : ICommand;
