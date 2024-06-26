using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Users.Authorization;
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
    public ExternalIdentityProvider? ExternalIdentityProvider { get; set; }
    public RefreshToken? RefreshToken { get; set; }
    public TwoFactorTokenHash? TwoFactorTokenHash { get; private set; }
    public TwoFactorToptSecret? TwoFactorToptSecret { get; private set; }
    public DateTimeOffset? TwoFactorTokenCreatedOn { get; private set; }
    public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();

    public static User Create(UserId id, Username username, Email email)
    {
        var user = new User(id, username, email);
        user.RaiseDomainEvent(UserRegisteredDomainEvent.New(user.Id));
        return user;
    }

    public static User New(Username username, Email email)
    {
        return Create(UserId.New(), username, email);
    }

    public void SetHashedPassword(PasswordHash passwordHash)
    {
        PasswordHash = passwordHash;
    }

    public void SetExternalIdentityProvider(ExternalIdentityProvider externalIdentityProvider)
    {
        ExternalIdentityProvider = externalIdentityProvider;
    }

    public void SetTwoFactorToken(TwoFactorTokenHash twoFactorToken, string twoFactorTokenAsString)
    {
        TwoFactorTokenHash = twoFactorToken;
        TwoFactorTokenCreatedOn = DateTimeOffset.UtcNow;
        RaiseDomainEvent(TwoFactorTokenCreatedDomainEvent.New(Id, twoFactorTokenAsString));
    }

    public void ClearTwoFactorToken()
    {
        TwoFactorTokenHash = null;
        TwoFactorTokenCreatedOn = null;
    }

    public void SetTwoFactorToptSecret(TwoFactorToptSecret twoFactorToptSecret)
    {
        TwoFactorToptSecret = twoFactorToptSecret;
    }

    public void Refresh(RefreshToken refreshToken)
    {
        RefreshToken = refreshToken;
    }

    public void Revoke()
    {
        RefreshToken = null;
    }

    public void AddRole(Role role)
    {
        _roles.Add(role);
    }

    public void RemoveRole(Role role)
    {
        _roles.Remove(role);
    }
}
