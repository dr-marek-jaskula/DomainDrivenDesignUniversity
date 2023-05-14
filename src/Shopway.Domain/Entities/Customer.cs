using Shopway.Domain.Enums;
using Shopway.Domain.EntityIds;
using Shopway.Domain.ValueObjects;
using Shopway.Domain.BaseTypes;
using Shopway.Domain.Abstractions;

namespace Shopway.Domain.Entities;

public sealed class Customer : Entity<CustomerId>, IAuditable
{
    private readonly List<Order> _orders = new();

    private Customer
    (
        CustomerId id,
        FirstName firstName,
        LastName lastName,
        Gender gender,
        DateOnly? dateOfBirth,
        PhoneNumber contactNumber,
        Address? address,
        User user,
        Rank rank
    )
        :base(id)
    {
        Rank = rank;
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        DateOfBirth = dateOfBirth;
        PhoneNumber = contactNumber;
        Address = address;
        User = user;
    }

    // Empty constructor in this case is required by EF Core
    private Customer()
    {
    }

    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public Gender Gender { get; private set; }
    public Rank Rank { get; private set; }

    //DateOnly property needs a conversion to SQL Server DATE format
    public DateOnly? DateOfBirth { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public Address? Address { get; private set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    public UserId UserId { get; private set; }
    public User User { get; private set; }
    public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();

    public static Customer Create
    (
        CustomerId id,
        FirstName firstName,
        LastName lastName,
        Gender gender,
        DateOnly? dateOfBirth,
        PhoneNumber contactNumber,
        Address? address,
        User user,
        Rank rank
    )
    {
        return new Customer
        (
            id,
            firstName,
            lastName,
            gender,
            dateOfBirth,
            contactNumber,
            address,
            user,
            rank
        );
    }
}