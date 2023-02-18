using Shopway.Domain.Abstractions;
using Shopway.Domain.BaseTypes;
using Shopway.Domain.DomainEvents;
using Shopway.Domain.Entities.Parents;
using Shopway.Domain.Enumerations;
using Shopway.Domain.EntityIds;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Entities;

public sealed class User : AggregateRoot<UserId>, IAuditable
{
    private readonly List<Role> _roles = new();

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
    public PersonId? PersonId { get; set; }
    public Person? Person { get; set; }
    public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();

    public static User Create(UserId id, Username username, Email email)
    {
        var user = new User(id, username, email);
        user.RaiseDomainEvent(new UserRegisteredDomainEvent(Guid.NewGuid(), user.Id));
        return user;
    }

    public void SetHashedPassword(PasswordHash passwordHash)
    {
        PasswordHash = passwordHash;
    }
}