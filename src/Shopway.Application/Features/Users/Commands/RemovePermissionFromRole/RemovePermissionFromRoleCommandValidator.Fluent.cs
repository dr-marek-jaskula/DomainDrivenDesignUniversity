using FluentValidation;

namespace Shopway.Application.Features.Users.Commands.RemovePermissionFromRole;

internal sealed class RemovePermissionFromRoleCommandValidator : AbstractValidator<RemovePermissionFromRoleCommand>
{
    public RemovePermissionFromRoleCommandValidator()
    {
        RuleFor(x => x.Role).NotEmpty();
        RuleFor(x => x.Permission).NotEmpty();
    }
}