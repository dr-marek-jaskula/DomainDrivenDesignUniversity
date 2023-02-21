using Shopway.Domain.Entities;
using Shopway.Application.CQRS.Users.Commands.RegisterUser;

namespace Shopway.Application.Mapping;

public static class UserMapping
{
    public static RegisterUserResponse ToCreateResponse(this User userToCreate)
    {
        return new RegisterUserResponse(userToCreate.Id.Value);
    }
}