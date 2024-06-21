namespace Shopway.Domain.Users.Authorization;

public sealed class RoleUser
{
    public RoleUser(string roleName, UserId userId)
    {
        RoleName = roleName;
        UserId = userId;
    }

    private RoleUser()
    {
    }

    public string RoleName { get; init; }

    public UserId UserId { get; init; }
}
