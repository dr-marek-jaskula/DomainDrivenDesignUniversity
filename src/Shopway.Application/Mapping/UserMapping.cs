using Shopway.Domain.Entities;
using Shopway.Application.CQRS.Users.Commands.CreateUser;

namespace Shopway.Application.Mapping;

public static class UserMapping
{
    public static CreateUserResponse ToCreateResponse(this User userToCreate)
    {
        return new CreateUserResponse(userToCreate.Id.Value);
    }
}