using Shopway.Domain.DomainEvents;
using Shopway.Domain.Entities.Parents;
using Shopway.Domain.Primitives;
using Shopway.Domain.StronglyTypedIds;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Entities;

public sealed class User : AggregateRoot<UserId>, IAuditableEntity
{
    public Username Username { get; set; }
    public Email Email { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public PasswordHash PasswordHash { get; set; }
    public PersonId? PersonId { get; set; }
    public Person? Person { get; set; }
    public ICollection<Role> Roles { get; set; }

    internal User
    (
        UserId id,
        Username username,
        Email email,
        PasswordHash passwordHash
    )
        : base(id)
    {
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
    }

    // Empty constructor in this case is required by EF Core
    private User()
    {
    }

    public static User Create
    (
        UserId id,
        Username username,
        Email email,
        PasswordHash passwordHash
    )
    {
        var user = new User
        (
            id,
            username,
            email,
            passwordHash
        );

        user.RaiseDomainEvent(new UserRegisteredDomainEvent(Guid.NewGuid(), user.Id));

        return user;
    }
}