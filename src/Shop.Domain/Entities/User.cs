using Shopway.Domain.Entities.Parents;
using Shopway.Domain.Primitives;
using Shopway.Domain.ValueObjects;
using System.Diagnostics.Metrics;

namespace Shopway.Domain.Entities;

public sealed class User : AggregateRoot, IAuditableEntity
{
    private User(
        Guid id, 
        string username, 
        string email, 
        string passwordHash) 
        : base(id)
    {
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
    }

    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public Role? Role { get; set; }
    public int? PersonId { get; set; }
    public Person? Person { get; set; }

    public static User Create(
    Guid id,
    Email email,
    FirstName firstName,
    LastName lastName)
    {
        var user = new User(
            id,
            email,
            firstName,
            lastName);

        return user;
    }

    public void ChangeName(FirstName username)
    {
        Username = username;
    }
}