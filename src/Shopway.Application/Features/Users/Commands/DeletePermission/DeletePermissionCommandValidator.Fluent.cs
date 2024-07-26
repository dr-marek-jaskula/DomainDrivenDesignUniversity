using FluentValidation;
using Shopway.Application.Utilities;
using Shopway.Domain.Users.Authorization;

namespace Shopway.Application.Features.Users.Commands.DeletePermission;

internal sealed class DeletePermissionCommandValidator : AbstractValidator<DeletePermissionCommand>
{
    public DeletePermissionCommandValidator()
    {
        RuleFor(x => x.PermissionName)
            .MustBeAnEnum<DeletePermissionCommand, PermissionName>();
    }
}
