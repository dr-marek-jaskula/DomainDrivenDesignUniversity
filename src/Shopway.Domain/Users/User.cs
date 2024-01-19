using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Users.Enumerations;
using Shopway.Domain.Users.Events;
using Shopway.Domain.Users.ValueObjects;

namespace Shopway.Domain.Users;

public sealed class User : AggregateRoot<UserId>, IAuditable
{
    private readonly List<Role> _roles = [];

    private User(UserId id, Username username, Email email)
        : base(id)
    {
        Username = username;
        Email = email;
    }

    // Empty constructor in this case is required by EF Core
    private User()
    {
    }

    public Username Username { get; set; }
    public Email Email { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public PasswordHash PasswordHash { get; set; }
    public CustomerId? CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();

    public static User Create(UserId id, Username username, Email email)
    {
        var user = new User(id, username, email);
        user.RaiseDomainEvent(UserRegisteredDomainEvent.New(user.Id));
        return user;
    }

    public void SetHashedPassword(PasswordHash passwordHash)
    {
        PasswordHash = passwordHash;
    }
}