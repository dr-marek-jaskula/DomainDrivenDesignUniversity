//using Shopway.Domain.Entities.Parents;
//using Shopway.Domain.Primitives;
//using Shopway.Domain.ValueObjects;

//namespace Shopway.Domain.Entities;

//public sealed class User : AggregateRoot, IAuditableEntity
//{
//    public Username Username { get; set; }
//    public Email Email { get; set; }
//    public DateTimeOffset CreatedOn { get; set; }
//    public DateTimeOffset? UpdatedOn { get; set; }
//    public PasswordHash PasswordHash { get; set; }
//    public Guid RoleId { get; set; }
//    public Role? Role { get; set; }
//    public Guid? PersonId { get; set; }
//    public Person? Person { get; set; }

//    internal User(
//        Guid id,
//        Username username,
//        Email email,
//        PasswordHash passwordHash)
//        : base(id)
//    {
//        Username = username;
//        Email = email;
//        PasswordHash = passwordHash;
//    }

//    // Empty constructor in this case is required by EF Core
//    private User()
//    {
//    }

//    public static User Create(
//        Guid id,
//        Username username,
//        Email email,
//        PasswordHash passwordHash)
//    {
//        var user = new User
//        (
//            id,
//            username,
//            email,
//            passwordHash
//        );

//        return user;
//    }
//}